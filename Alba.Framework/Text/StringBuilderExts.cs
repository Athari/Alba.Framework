using System.Text;

namespace Alba.Framework.Text;

public static class StringBuilderExts
{
    extension(StringBuilder @this)
    {
        public void AppendSentence(string sentence)
        {
            if (@this.Length > 0)
                @this.Append(@this[^1] == '.' ? " " : ". ");
            @this.Append(sentence);
        }
    }
}