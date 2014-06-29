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
        private static readonly char[] EventMethodSeparators = { '=' };

        private static void AttachMany_Changed (DependencyObject d, DpChangedEventArgs<string> args)
        {
            CaliburnMicro.Message.SetAttach(d, args.NewValue
                .Split(AttachSeparators, StringSplitOptions.RemoveEmptyEntries)
                .Where(a => !a.IsNullOrWhitespace())
                .Select(GetAttachMessageItem)
                .JoinString("; "));
        }

        private static string GetAttachMessageItem (string action)
        {
            if (action.IndexOf(EventMethodSeparators[0]) != -1) {
                string[] eventAction = action.Split(EventMethodSeparators, 2);
                return "[Event {0}] = [Action {1}]".FmtInv(eventAction[0], eventAction[1]);
            }
            else {
                return "[Event {0}] = [Action {1}]".FmtInv(GetAttachMessageEvent(action), action);
            }
        }

        private static string GetAttachMessageEvent (string attach)
        {
            int openParen = attach.IndexOf('(');
            return openParen == -1 ? attach : attach.Remove(openParen);
        }
    }
}