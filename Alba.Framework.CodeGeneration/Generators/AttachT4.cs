extern alias vs11;
extern alias vs12;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using EnvDTE;

namespace Alba.Framework.CodeGeneration.Generators
{
    [ComVisible (true), Guid ("D00DBAEC-07BE-442F-8D8F-C554084CA634")]
    [CustomTool (AttachT4Generator.ToolName, AttachT4Generator.ToolDescription, "11.0")]
    public class AttachT4VS11 : vs11::Microsoft.VisualStudio.TextTemplating.VSHost.BaseCodeGeneratorWithSite
    {
        private readonly AttachT4Generator _generator = new AttachT4Generator();

        public override string GetDefaultExtension ()
        {
            return _generator.FileExtension;
        }

        protected override byte[] GenerateCode (string csName, string csText)
        {
            return _generator.GenerateCode(csName, csText, Dte,
                vs11::Microsoft.VisualStudio.TextTemplating.EncodingHelper.GetEncoding);
        }

        [ComRegisterFunction, ComVisible(false)]
        public static void Register (Type t)
        {
            VisualStudioRegistrator.RegisterGenerator(t);
        }

        [ComUnregisterFunction, ComVisible(false)]
        public static void Unregister (Type t)
        {
            VisualStudioRegistrator.UnregisterGenerator(t);
        }
    }

    [ComVisible (true), Guid ("21FC238D-6F2A-449B-9531-9552A24A75A4")]
    [CustomTool (AttachT4Generator.ToolName, AttachT4Generator.ToolDescription, "12.0")]
    public class AttachT4VS12 : vs12::Microsoft.VisualStudio.TextTemplating.VSHost.BaseCodeGeneratorWithSite
    {
        private readonly AttachT4Generator _generator = new AttachT4Generator();

        public override string GetDefaultExtension ()
        {
            return _generator.FileExtension;
        }

        protected override byte[] GenerateCode (string csName, string csText)
        {
            return _generator.GenerateCode(csName, csText, Dte,
                vs12::Microsoft.VisualStudio.TextTemplating.EncodingHelper.GetEncoding);
        }

        [ComRegisterFunction, ComVisible(false)]
        public static void Register (Type t)
        {
            VisualStudioRegistrator.RegisterGenerator(t);
        }

        [ComUnregisterFunction, ComVisible(false)]
        public static void Unregister (Type t)
        {
            VisualStudioRegistrator.UnregisterGenerator(t);
        }
    }

    internal class AttachT4Generator
    {
        public const string ToolName = "AttachT4";
        public const string ToolDescription = "Attach T4 file";

        public string FileExtension
        {
            get { return ".tt"; }
        }

        public byte[] GenerateCode (string csName, string csText, DTE dte, Func<string, Encoding> getEncoding)
        {
            string ttName = Path.ChangeExtension(csName, ".tt");

            Document ttDoc = dte.Documents.Cast<Document>().SingleOrDefault(d => d.FullName == ttName);
            if (ttDoc != null && !ttDoc.Saved)
                ttDoc.Save();

            Encoding ttEncoding;
            string ttText;

            if (File.Exists(ttName)) {
                ttEncoding = getEncoding(ttName);
                ttText = File.ReadAllText(ttName, ttEncoding);
            }
            else {
                ttEncoding = Encoding.UTF8;
                ttText = ("<#@ template hostspecific='true' debug='true' #>\n" +
                    "<#@ output extension='/' #>\n" +
                    "<#@ include file='$(SolutionDir)/Alba.Framework.CodeGeneration/Common.tt' #>\n")
                    .Replace("\n", Environment.NewLine).Replace('\'', '"');
            }

            return ttEncoding.GetBytes(ttText);
        }
    }
}