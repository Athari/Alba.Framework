using System;
using System.Linq;
using System.Windows;
using Alba.Framework.Collections;
using Alba.Framework.Text;
using Alba.Framework.Windows;
using CaliburnMicro = Caliburn.Micro;

namespace Alba.Framework.Caliburn
{
    public static partial class Message
    {
        private static readonly char[] AttachSeparators = { ';' };

        private static void AttachMany_Changed (DependencyObject d, DpChangedEventArgs<string> args)
        {
            CaliburnMicro.Message.SetAttach(d, args.NewValue
                .Split(AttachSeparators, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => "[Event {0}] = [Action {1}]".FmtInv(GetAttachMethod(a), a))
                .JoinString("; "));
        }

        private static string GetAttachMethod (string attach)
        {
            int openParen = attach.IndexOf('(');
            return openParen == -1 ? attach : attach.Remove(openParen);
        }
    }
}