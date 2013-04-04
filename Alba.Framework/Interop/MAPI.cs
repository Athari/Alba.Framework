using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using Alba.Framework.Text;

namespace Alba.Framework.Interop
{
    public class Mapi
    {
        public Mapi (string subject, string body, IEnumerable<string> to = null, IEnumerable<string> cc = null,
            IEnumerable<string> bcc = null, IEnumerable<string> attachments = null)
        {
            Subject = subject;
            Body = body;
            To = to != null ? to.ToList() : new List<string>();
            CC = cc != null ? cc.ToList() : new List<string>();
            BCC = bcc != null ? bcc.ToList() : new List<string>();
            Attachments = attachments != null ? attachments.ToList() : new List<string>();
        }

        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> BCC { get; set; }
        public List<string> Attachments { get; set; }

        public void SendMail ()
        {
            var recipients = new List<MapiRecipDesc>();
            To.ForEach(email => recipients.Add(new MapiRecipDesc { recipClass = RecipClass.To, name = email }));
            CC.ForEach(email => recipients.Add(new MapiRecipDesc { recipClass = RecipClass.CC, name = email }));
            BCC.ForEach(email => recipients.Add(new MapiRecipDesc { recipClass = RecipClass.BCC, name = email }));

            var msg = new MapiMessage {
                subject = Subject, noteText = Body,
                recips = GetRecipientsData(recipients), recipCount = recipients.Count,
                files = GetAttachmentsData(Attachments), fileCount = Attachments.Count,
            };

            int result = MAPISendMail(IntPtr.Zero, IntPtr.Zero, msg, MAPI_LOGON_UI | MAPI_DIALOG, 0);
            if (result > 1)
                throw new Exception("Failed to send mail: {0}.".Fmt(GetErrorMessage(result)));
            Cleanup(msg);
        }

        private static IntPtr GetRecipientsData (List<MapiRecipDesc> recipients)
        {
            if (recipients.Count == 0)
                return IntPtr.Zero;

            IntPtr ptr = Native.AllocHGlobalArray<MapiRecipDesc>(recipients.Count), iptr = ptr;
            foreach (MapiRecipDesc recipient in recipients)
                Native.StructureToPtrInc(recipient, ref iptr);
            return ptr;
        }

        private static IntPtr GetAttachmentsData (List<string> attachments)
        {
            if (attachments.Count == 0)
                return IntPtr.Zero;

            IntPtr ptr = Native.AllocHGlobalArray<MapiFileDesc>(attachments.Count), iptr = ptr;
            var attachment = new MapiFileDesc { position = -1 };
            foreach (string fileName in attachments) {
                attachment.name = Path.GetFileName(fileName);
                attachment.path = fileName;
                Native.StructureToPtrInc(attachment, ref iptr);
            }
            return ptr;
        }

        private static void Cleanup (MapiMessage msg)
        {
            Native.FreeHGlobalArray<MapiRecipDesc>(msg.recips, msg.recipCount);
            Native.FreeHGlobalArray<MapiFileDesc>(msg.files, msg.fileCount);
        }

        public string GetErrorMessage (int result)
        {
            return result < ErrorMessages.Length ? ErrorMessages[result] : "MAPI error [{0}]".Fmt(result);
        }

        private const int MAPI_LOGON_UI = 0x00000001;
        private const int MAPI_DIALOG = 0x00000008;
        private static readonly string[] ErrorMessages = new[] {
            "OK [0]", "User abort [1]", "General MAPI failure [2]", "MAPI login failure [3]", "Disk full [4]",
            "Insufficient memory [5]", "Access denied [6]", "-unknown- [7]", "Too many sessions [8]",
            "Too many files were specified [9]", "Too many recipients were specified [10]",
            "A specified attachment was not found [11]", "Attachment open failure [12]",
            "Attachment write failure [13]", "Unknown recipient [14]", "Bad recipient type [15]", "No messages [16]",
            "Invalid message [17]", "Text too large [18]", "Invalid session [19]", "Type not supported [20]",
            "A recipient was specified ambiguously [21]", "Message in use [22]", "Network failure [23]",
            "Invalid edit fields [24]", "Invalid recipients [25]", "Not supported [26]",
        };

        private enum RecipClass
        {
            To = 1,
            CC,
            BCC
        };

        [DllImport ("MAPI32.dll")]
        private static extern int MAPISendMail (IntPtr sess, IntPtr hwnd, MapiMessage message, int flg, int rsv);

// ReSharper disable NotAccessedField.Local
#pragma warning disable 169
        [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class MapiMessage
        {
            public int reserved;
            public string subject;
            public string noteText;
            public string messageType;
            public string dateReceived;
            public string conversationID;
            public int flags;
            public IntPtr originator;
            public int recipCount;
            public IntPtr recips;
            public int fileCount;
            public IntPtr files;
        }

        [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class MapiFileDesc
        {
            public int reserved;
            public int flags;
            public int position;
            public string path;
            public string name;
            public IntPtr type;
        }

        [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class MapiRecipDesc
        {
            public int reserved;
            public RecipClass recipClass;
            public string name;
            public string address;
            public int eIDSize;
            public IntPtr entryID;
        }
#pragma warning restore 169
// ReSharper restore NotAccessedField.Local
    }
}