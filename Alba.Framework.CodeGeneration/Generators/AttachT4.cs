using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace Alba.Framework.CodeGeneration.Generators
{
    [Guid ("D00DBAEC-07BE-442F-8D8F-C554084CA634"), CustomTool ("AttachT4", "Attach T4")]
    public class AttachT4 : BaseCodeGeneratorWithSite
    {
        public override string GetDefaultExtension ()
        {
            return ".tt";
        }

        protected override byte[] GenerateCode (string csName, string csText)
        {
            string ttName = Path.ChangeExtension(csName, ".tt");

            // If T4 script file is open in the editor, save it
            Document ttDoc = Dte.Documents.Cast<Document>().SingleOrDefault(d => d.FullName == ttName);
            if (ttDoc != null && !ttDoc.Saved)
                ttDoc.Save();

            Encoding ttEncoding;
            string ttText;

            if (File.Exists(ttName)) {
                ttEncoding = EncodingHelper.GetEncoding(ttName);
                ttText = File.ReadAllText(ttName, ttEncoding);
            }
            else {
                ttEncoding = Encoding.UTF8;
                ttText = ("<#@ template hostspecific='true' debug='true' #>\n" +
                    "<#@ output extension='txt' #>\n" +
                    "<#@ include file='$(SolutionDir)/Alba.Framework.CodeGeneration/Common.tt' #>\n")
                    .Replace("\n", Environment.NewLine).Replace('\'', '"');
            }

            return ttEncoding.GetBytes(ttText);
        }

        [ComRegisterFunction]
        public static void Register (Type t)
        {
            Registrator.RegisterGenerator(t);
        }

        [ComUnregisterFunction]
        public static void Unregister (Type t)
        {
            Registrator.UnregisterGenerator(t);
        }
    }
}