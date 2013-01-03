namespace Alba.Framework.Logs
{
    public class LogEntry
    {
        public object Data { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }

        public override string ToString ()
        {
            return string.Format("{0}\r\n{1}", Message, DetailedMessage);
        }
    }
}