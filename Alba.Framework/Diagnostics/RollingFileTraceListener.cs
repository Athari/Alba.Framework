using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Alba.Framework.Text;

namespace Alba.Framework.Diagnostics;

[PublicAPI]
public class RollingFileTraceListener : TextWriterTraceListener
{
    private const int SourceNameFormatLen = 20;
    private const int TypeNameFormatLen = 40;
    private static readonly string SourceTypeNamesFormat = $"{{0,{SourceNameFormatLen}}}:{{1,{TypeNameFormatLen}}}";

    private readonly RollInterval _rollInterval;
    private readonly int _rollSize;
    private readonly string? _timeStampPattern;
    private readonly int _maxArchivedFiles;
    private readonly RollingHelper _rollingHelper;
    private readonly StringBuilder _currentLine;

    public RollingFileTraceListener(string fileName, int rollSize = 0,
        RollInterval rollInterval = RollInterval.None, string? timeStampPattern = null, int maxArchivedFiles = 0)
        : base(fileName)
    {
        _rollSize = rollSize;
        _rollInterval = rollInterval;
        _timeStampPattern = timeStampPattern;
        _maxArchivedFiles = maxArchivedFiles;
        _rollingHelper = new(this);
        _currentLine = new();
    }

    public override void Write(string? message)
    {
        AppendLine(message ?? "");
    }

    public override void WriteLine(string? message)
    {
        AppendLine(message ?? "");
        NeedIndent = true;
        TraceEvent(new(), "Trace", TraceEventType.Verbose, 0, message != null ? message.Trim() : "");
        _currentLine.Clear();
    }

    private void AppendLine(string message)
    {
        if (NeedIndent)
            WriteIndent();
        _currentLine.Append(message);
    }

    public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, object? data)
    {
        if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null)) {
            if (data is LogEntry entry) {
                WriteMessage(eventCache, FormatSource(source, entry.MediumTypeName), eventType, id,
                    $"{entry.Message}\n{entry.DetailedMessage}");
                return;
            }
            WriteMessage(eventCache, source, eventType, id, data?.ToString() ?? "");
        }
    }

    public override void TraceData(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, params object?[]? data)
    {
        if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data)) {
            var sb = new StringBuilder();
            if (data != null) {
                for (int i = 0; i < data.Length; i++) {
                    if (i != 0)
                        sb.Append(", ");
                    if (data[i] != null)
                        sb.Append(data[i]);
                }
            }
            WriteMessage(eventCache, FormatSource(source), eventType, id, sb.ToString());
        }
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
    {
        if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            WriteMessage(eventCache, FormatSource(source), eventType, id, message);
    }

    public override void TraceEvent(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? format, params object?[]? args)
    {
        if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            WriteMessage(eventCache, FormatSource(source), eventType, id, format != null && args != null ? string.Format(format, args) : format);
    }

    private void WriteMessage(TraceEventCache? eventCache, string source, TraceEventType eventType, int id, string? message)
    {
        _rollingHelper.RollIfNecessary();
        if (source == "Trace" && eventType == TraceEventType.Verbose)
            return;
        message = message?.Trim() ?? "";
        string threadIdPrefix = string.Format(CultureInfo.InvariantCulture, "[{0}] ", Environment.CurrentManagedThreadId);
        if (message.StartsWith(threadIdPrefix))
            message = message[threadIdPrefix.Length..];
        message = string.Format(id != 0 ? "[{0:u}] [{1}] <{2}#{3}> {4}" : "[{0:u}] [{1}] <{2}> {4}",
            eventCache?.DateTime, EventTypeToShortString(eventType), source, id, message);
        base.WriteLine(message);
        Flush();
    }

    private static string FormatSource(string sourceName, string? typeName = null)
    {
        return string.Format(SourceTypeNamesFormat,
            sourceName.SubstringEnd(SourceNameFormatLen), (typeName ?? "?").SubstringEnd(TypeNameFormatLen));
    }

    private static string EventTypeToShortString(TraceEventType eventType)
    {
        return eventType switch {
            TraceEventType.Critical => "CRIT",
            TraceEventType.Error => "ERRO",
            TraceEventType.Warning => "WARN",
            TraceEventType.Information => "INFO",
            TraceEventType.Verbose => "TRAC",
            _ => eventType.ToString(),
        };
    }

    private class RollingHelper
    {
        private readonly RollingFileTraceListener _owner;
        private readonly bool _performsRolling;
        private TallyStreamWriter? _tallyWriter;
        private DateTime? _nextRollDateTime;

        public void RollIfNecessary()
        {
            // avoid further processing if no rolling has been configured.
            if (!_performsRolling)
                return;
            // an error was detected while handling roll information - avoid further processing
            if (!UpdateRollingInformationIfNecessary())
                return;

            DateTime? rollDateTime;
            if ((rollDateTime = CheckIsRollNecessary()) != null)
                PerformRoll(rollDateTime.Value);
        }

        public RollingHelper(RollingFileTraceListener owner)
        {
            _owner = owner;
            _performsRolling = _owner._rollInterval != RollInterval.None || _owner._rollSize > 0;
        }

        private DateTime CalculateNextRollDate(DateTime dateTime)
        {
            return _owner._rollInterval switch {
                RollInterval.None => DateTime.MaxValue,
                RollInterval.Hour => dateTime.AddHours(1),
                RollInterval.Day => dateTime.AddDays(1).Date,
                RollInterval.Week => dateTime.AddDays(7).Date,
                RollInterval.Month => dateTime.AddMonths(1).Date,
                RollInterval.Year => dateTime.AddYears(1).Date,
                _ => throw new ArgumentOutOfRangeException(nameof(dateTime)),
            };
        }

        private DateTime? CheckIsRollNecessary()
        {
            // check for size roll, if enabled.
            if (_owner._rollSize > 0
             && (_tallyWriter != null && _tallyWriter.Tally > _owner._rollSize)) {
                return DateTime.Now;
            }

            // check for date roll, if enabled.
            DateTime currentDateTime = DateTime.Now;
            if (_owner._rollInterval != RollInterval.None
             && (_nextRollDateTime != null && currentDateTime.CompareTo(_nextRollDateTime.Value) >= 0)) {
                return currentDateTime;
            }

            // no roll is necessary, return a null roll date
            return null;
        }

        private string ComputeArchiveFileName(string actualFileName, DateTime currentDateTime)
        {
            string directory = Path.GetDirectoryName(actualFileName)!;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(actualFileName);
            string extension = Path.GetExtension(actualFileName);

            var fileNameBuilder = new StringBuilder(fileNameWithoutExtension);
            if (!string.IsNullOrEmpty(_owner._timeStampPattern)) {
                fileNameBuilder.Append('.');
                fileNameBuilder.Append(currentDateTime.ToString(_owner._timeStampPattern, CultureInfo.InvariantCulture));
            }

            // look for max sequence for date
            int newSequence = FindMaxSequenceNumber(directory, fileNameBuilder.ToString(), extension) + 1;
            fileNameBuilder.Append('.');
            fileNameBuilder.Append(newSequence.ToString(CultureInfo.InvariantCulture));

            fileNameBuilder.Append(extension);

            return Path.Combine(directory, fileNameBuilder.ToString());
        }

        private static int FindMaxSequenceNumber(string directoryName, string fileName, string extension)
        {
            string[] existingFiles = Directory.GetFiles(directoryName,
                $"{fileName}*{extension}");

            int maxSequence = 0;
            var regex = new Regex($@"{fileName}\.(?<sequence>\d+){extension}$");
            foreach (string existingFile in existingFiles) {
                Match sequenceMatch = regex.Match(existingFile);
                if (!sequenceMatch.Success)
                    continue;
                string sequenceInFile = sequenceMatch.Groups["sequence"].Value;
                if (!int.TryParse(sequenceInFile, out int currentSequence))
                    continue; // very unlikely
                if (currentSequence > maxSequence)
                    maxSequence = currentSequence;
            }

            return maxSequence;
        }

        private void PerformRoll(DateTime rollDateTime)
        {
            string actualFileName = ((FileStream)((StreamWriter)_owner.Writer!).BaseStream).Name;

            string archiveFileName = ComputeArchiveFileName(actualFileName, rollDateTime);
            _owner.Writer.Close();
            SafeMove(actualFileName, archiveFileName, rollDateTime);
            PurgeArchivedFiles(actualFileName);

            // update writer - let TWTL open the file as needed to keep consistency
            _owner.Writer = null;
            _tallyWriter = null;
            _nextRollDateTime = null;
            UpdateRollingInformationIfNecessary();
        }

        private static void SafeMove(string actualFileName, string archiveFileName, DateTime currentDateTime)
        {
            try {
                if (File.Exists(archiveFileName))
                    File.Delete(archiveFileName);
                // take care of tunneling issues http://support.microsoft.com/kb/172190
                File.SetCreationTime(actualFileName, currentDateTime);
                File.Move(actualFileName, archiveFileName);
            }
            catch (IOException) {
                // catch errors and attempt move to a new file with a GUID
                archiveFileName += Guid.NewGuid();
                try {
                    File.Move(actualFileName, archiveFileName);
                }
                catch (IOException) { }
            }
        }

        private void PurgeArchivedFiles(string actualFileName)
        {
            if (_owner._maxArchivedFiles <= 0)
                return;
            var directoryName = Path.GetDirectoryName(actualFileName)!;
            var fileName = Path.GetFileName(actualFileName);
            new RollingFilePurger(directoryName, fileName, _owner._maxArchivedFiles).Purge();
        }

        private bool UpdateRollingInformationIfNecessary()
        {
            // replace writer with the tally keeping version if necessary for size rolling
            if (_owner._rollSize > 0 && _tallyWriter == null) {
                if (_owner.Writer is not StreamWriter currentWriter) {
                    // couldn't acquire the writer - abort
                    return false;
                }
                string actualFileName = ((FileStream)currentWriter.BaseStream).Name;

                currentWriter.Close();

                try {
                    FileStream fileStream = File.Open(actualFileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                    _tallyWriter = new(fileStream);
                }
                catch (Exception) {
                    // there's a slight chance of error here - abort if this occurs and just let TWTL handle it without attempting to roll
                    return false;
                }

                _owner.Writer = _tallyWriter;
            }

            // compute the next roll date if necessary
            if (_owner._rollInterval != RollInterval.None && _nextRollDateTime == null) {
                try {
                    // casting should be safe at this point - only file stream writers can be the writers for the owner trace listener.
                    // it should also happen rarely
                    _nextRollDateTime = CalculateNextRollDate(
                        File.GetCreationTime(((FileStream)((StreamWriter)_owner.Writer!).BaseStream).Name));
                }
                catch (Exception) {
                    _nextRollDateTime = DateTime.MaxValue; // disable rolling if not date could be retrieved.
                    // there's a slight chance of error here - abort if this occurs and just let TWTL handle it without attempting to roll
                    return false;
                }
            }

            return true;
        }
    }

    private class TallyStreamWriter : StreamWriter
    {
        public long Tally { get; private set; }

        private static Encoding Utf8WithFallback
        {
            get
            {
                var encoding = (Encoding)new UTF8Encoding(false).Clone();
                encoding.EncoderFallback = EncoderFallback.ReplacementFallback;
                encoding.DecoderFallback = DecoderFallback.ReplacementFallback;
                return encoding;
            }
        }

        public TallyStreamWriter(Stream stream) : this(stream, Utf8WithFallback) { }

        private TallyStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
            Tally = stream.Length;
        }

        public override void Write(char value)
        {
            base.Write(value);
            Tally += Encoding.GetByteCount([value]);
        }

        public override void Write(char[]? buffer)
        {
            base.Write(buffer);
            Tally += buffer != null ? Encoding.GetByteCount(buffer) : 0;
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
            Tally += Encoding.GetByteCount(buffer, index, count);
        }

        public override void Write(string? value)
        {
            base.Write(value);
            Tally += value != null ? Encoding.GetByteCount(value) : 0;
        }
    }

    private class RollingFilePurger(string directory, string baseFileName, int maxFiles)
    {
        public void Purge()
        {
            var extension = Path.GetExtension(baseFileName);
            var searchPattern = Path.GetFileNameWithoutExtension(baseFileName) + ".*" + extension;

            string[] matchingFiles = TryGetMatchingFiles(searchPattern);

            // bail out early if possible
            if (matchingFiles.Length <= maxFiles)
                return;

            // sort the archive files in descending order by creation date and sequence number
            matchingFiles
                .Select(matchingFile => new ArchiveFile(matchingFile))
                .OrderByDescending(archiveFile => archiveFile)
                .Skip(maxFiles)
                .ToList()
                .ForEach(file => file.TryDelete());
        }

        private string[] TryGetMatchingFiles(string searchPattern)
        {
            try {
                return Directory.GetFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }

            return [ ];
        }

        private class ArchiveFile : IComparable<ArchiveFile>
        {
            private readonly string _path;
            private readonly DateTime _creationTime;
            private string? _sequenceString;
            private int? _sequence;

            public ArchiveFile(string path)
            {
                _path = path;
                SequenceString = Path.GetFileName(path);
                _creationTime = GetCreationTime(path);
            }

            private string SequenceString
            {
                get { return _sequenceString ??= GetSequence(field); }
            }

            private int Sequence
            {
                get
                {
                    _sequence ??= int.TryParse(SequenceString, NumberStyles.None, CultureInfo.InvariantCulture, out int sequence) ? sequence : 0;
                    return _sequence.Value;
                }
            }

            public int CompareTo(ArchiveFile? other)
            {
                var creationDateComparison = _creationTime.CompareTo(other?._creationTime);
                if (creationDateComparison != 0)
                    return creationDateComparison;
                if (Sequence != 0 && other?.Sequence != 0) // both archive files have proper sequences - use them
                    return Sequence.CompareTo(other?.Sequence);
                else // compare the sequence part of the file name as plain strings
                    return string.Compare(SequenceString, other?.SequenceString, StringComparison.Ordinal);
            }

            public void TryDelete()
            {
                try {
                    File.Delete(_path);
                }
                catch (UnauthorizedAccessException) { } // cannot delete the file because of a permissions issue - just skip it
                catch (IOException) { } // cannot delete the file, most likely because it is already opened - just skip it
            }
        }

        private static DateTime GetCreationTime(string path)
        {
            try {
                return File.GetCreationTimeUtc(path);
            }
            catch (UnauthorizedAccessException) {
                // will cause file be among the first files when sorting, 
                // and its deletion will likely fail causing it to be skipped
                return DateTime.MinValue;
            }
        }

        private static string GetSequence(string fileName)
        {
            int extensionDotIndex = fileName.LastIndexOf('.');
            if (extensionDotIndex <= 0) // no dots - can't extract sequence
                return "";
            int sequenceDotIndex = fileName.LastIndexOf('.', extensionDotIndex - 1);
            if (sequenceDotIndex < 0) // single dot - can't extract sequence
                return "";
            return fileName.Substring(sequenceDotIndex + 1, extensionDotIndex - sequenceDotIndex - 1);
        }
    }
}

public enum RollInterval
{
    None,
    Hour,
    Day,
    Week,
    Month,
    Year,
};