using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Windows.Win32;

namespace Alba.Framework.Mail;

[PublicAPI]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public partial class Mapi(
    string subject, string body, IEnumerable<string>? to = null, IEnumerable<string>? cc = null,
    IEnumerable<string>? bcc = null, IEnumerable<string>? attachments = null)
{
    public string Subject { get; set; } = subject;
    public string Body { get; set; } = body;
    public List<string> To { get; set; } = to != null ? [ .. to ] : [ ];
    public List<string> CarbonCopy { get; set; } = cc != null ? [ .. cc ] : [ ];
    public List<string> BlindCarbonCopy { get; set; } = bcc != null ? [ .. bcc ] : [ ];
    public List<string> Attachments { get; set; } = attachments != null ? [ .. attachments ] : [ ];

    public void SendMail()
    {
        var recipients = new List<MapiRecipDesc>();
        To.ForEach(email => recipients.Add(new() { recipClass = RecipClass.To, name = email }));
        CarbonCopy.ForEach(email => recipients.Add(new() { recipClass = RecipClass.CC, name = email }));
        BlindCarbonCopy.ForEach(email => recipients.Add(new() { recipClass = RecipClass.BCC, name = email }));

        var msg = new MapiMessage {
            subject = Subject, noteText = Body,
            recips = GetRecipientsData(recipients), recipCount = recipients.Count,
            files = GetAttachmentsData(Attachments), fileCount = Attachments.Count,
        };

        int result = MAPISendMail(0, 0, msg, MAPI_LOGON_UI | MAPI_DIALOG, 0);
        if (result > 1)
            throw new($"Failed to send mail: {GetErrorMessage(result)}.");
        Cleanup(msg);
    }

    private static nint GetRecipientsData(List<MapiRecipDesc> recipients)
    {
        if (recipients.Count == 0)
            return 0;

        nint ptr = Native.AllocHGlobalArray<MapiRecipDesc>(recipients.Count), iptr = ptr;
        foreach (MapiRecipDesc recipient in recipients)
            Native.StructureToPtrInc(recipient, ref iptr);
        return ptr;
    }

    private static nint GetAttachmentsData(List<string> attachments)
    {
        if (attachments.Count == 0)
            return 0;

        nint ptr = Native.AllocHGlobalArray<MapiFileDesc>(attachments.Count), iptr = ptr;
        var attachment = new MapiFileDesc { position = -1 };
        foreach (string fileName in attachments) {
            attachment.name = Path.GetFileName(fileName);
            attachment.path = fileName;
            Native.StructureToPtrInc(attachment, ref iptr);
        }
        return ptr;
    }

    private static void Cleanup(MapiMessage msg)
    {
        Native.FreeHGlobalArray<MapiRecipDesc>(msg.recips, msg.recipCount);
        Native.FreeHGlobalArray<MapiFileDesc>(msg.files, msg.fileCount);
    }

    public static string GetErrorMessage(int result)
    {
        return result < ErrorMessages.Length ? ErrorMessages[result] : $"MAPI error [{result}]";
    }

    private const int MAPI_LOGON_UI = 0x00000001;
    private const int MAPI_DIALOG = 0x00000008;
    private static readonly string[] ErrorMessages = [
        "OK [0]", "User abort [1]", "General MAPI failure [2]", "MAPI login failure [3]", "Disk full [4]",
        "Insufficient memory [5]", "Access denied [6]", "-unknown- [7]", "Too many sessions [8]",
        "Too many files were specified [9]", "Too many recipients were specified [10]",
        "A specified attachment was not found [11]", "Attachment open failure [12]",
        "Attachment write failure [13]", "Unknown recipient [14]", "Bad recipient type [15]", "No messages [16]",
        "Invalid message [17]", "Text too large [18]", "Invalid session [19]", "Type not supported [20]",
        "A recipient was specified ambiguously [21]", "Message in use [22]", "Network failure [23]",
        "Invalid edit fields [24]", "Invalid recipients [25]", "Not supported [26]",
    ];

    private enum RecipClass
    {
        To = 1,
        CC,
        BCC,
    };

    [DllImport("mapi32")]
    private static extern int MAPISendMail(nint sess, nint hwnd, MapiMessage message, int flg, int rsv);

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
// ReSharper disable NotAccessedField.Local

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private class MapiMessage
    {
        public int reserved;
        public string subject;
        public string noteText;
        public string messageType;
        public string dateReceived;
        public string conversationID;
        public int flags;
        public nint originator;
        public int recipCount;
        public nint recips;
        public int fileCount;
        public nint files;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private class MapiFileDesc
    {
        public int reserved;
        public int flags;
        public int position;
        public string path;
        public string name;
        public nint type;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private class MapiRecipDesc
    {
        public int reserved;
        public RecipClass recipClass;
        public string name;
        public string address;
        public int eIDSize;
        public nint entryID;
    }
}