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
            action = action.Trim();
            if (action.IndexOf(EventMethodSeparators[0]) != -1) {
                string[] triggerAction = action.Split(EventMethodSeparators, 2);
                if (triggerAction[0].StartsWith("["))
                    return "[Key {0}] = [Action {1}]".FmtInv(triggerAction[0].RemoveSuffixes("[", "]"), triggerAction[1]); // "[Ctrl+O]=FileOpen"
                else
                    return "[Event {0}] = [Action {1}]".FmtInv(triggerAction[0], triggerAction[1]); // "MouseDown=ButtonMouseDown"
            }
            else
                return "[Event {0}] = [Action {1}]".FmtInv(GetAttachMessageEvent(action), action); // "MouseDown"
        }

        private static string GetAttachMessageEvent (string attach)
        {
            int openParen = attach.IndexOf('(');
            return openParen == -1 ? attach : attach.Remove(openParen);
        }
    }
}