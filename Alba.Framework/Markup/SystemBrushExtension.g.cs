using System;
using System.Windows;
using Alba.Framework.Text;

namespace Alba.Framework.Markup
{
    public class SystemBrushExtension : DynamicResourceExtension
    {
        public SystemBrushExtension (SystemBrushKey colorKey)
            : base(GetColorResourceKey(colorKey))
        {}

        private static object GetColorResourceKey (SystemBrushKey colorKey)
        {
            switch (colorKey) {
                case SystemBrushKey.ActiveBorder:
                    return SystemColors.ActiveBorderBrushKey;
                case SystemBrushKey.ActiveCaption:
                    return SystemColors.ActiveCaptionBrushKey;
                case SystemBrushKey.ActiveCaptionText:
                    return SystemColors.ActiveCaptionTextBrushKey;
                case SystemBrushKey.AppWorkspace:
                    return SystemColors.AppWorkspaceBrushKey;
                case SystemBrushKey.Control:
                    return SystemColors.ControlBrushKey;
                case SystemBrushKey.ControlDark:
                    return SystemColors.ControlDarkBrushKey;
                case SystemBrushKey.ControlDarkDark:
                    return SystemColors.ControlDarkDarkBrushKey;
                case SystemBrushKey.ControlLight:
                    return SystemColors.ControlLightBrushKey;
                case SystemBrushKey.ControlLightLight:
                    return SystemColors.ControlLightLightBrushKey;
                case SystemBrushKey.ControlText:
                    return SystemColors.ControlTextBrushKey;
                case SystemBrushKey.Desktop:
                    return SystemColors.DesktopBrushKey;
                case SystemBrushKey.GradientActiveCaption:
                    return SystemColors.GradientActiveCaptionBrushKey;
                case SystemBrushKey.GradientInactiveCaption:
                    return SystemColors.GradientInactiveCaptionBrushKey;
                case SystemBrushKey.GrayText:
                    return SystemColors.GrayTextBrushKey;
                case SystemBrushKey.Highlight:
                    return SystemColors.HighlightBrushKey;
                case SystemBrushKey.HighlightText:
                    return SystemColors.HighlightTextBrushKey;
                case SystemBrushKey.HotTrack:
                    return SystemColors.HotTrackBrushKey;
                case SystemBrushKey.InactiveBorder:
                    return SystemColors.InactiveBorderBrushKey;
                case SystemBrushKey.InactiveCaption:
                    return SystemColors.InactiveCaptionBrushKey;
                case SystemBrushKey.InactiveCaptionText:
                    return SystemColors.InactiveCaptionTextBrushKey;
                case SystemBrushKey.Info:
                    return SystemColors.InfoBrushKey;
                case SystemBrushKey.InfoText:
                    return SystemColors.InfoTextBrushKey;
                case SystemBrushKey.Menu:
                    return SystemColors.MenuBrushKey;
                case SystemBrushKey.MenuBar:
                    return SystemColors.MenuBarBrushKey;
                case SystemBrushKey.MenuHighlight:
                    return SystemColors.MenuHighlightBrushKey;
                case SystemBrushKey.MenuText:
                    return SystemColors.MenuTextBrushKey;
                case SystemBrushKey.ScrollBar:
                    return SystemColors.ScrollBarBrushKey;
                case SystemBrushKey.Window:
                    return SystemColors.WindowBrushKey;
                case SystemBrushKey.WindowFrame:
                    return SystemColors.WindowFrameBrushKey;
                case SystemBrushKey.WindowText:
                    return SystemColors.WindowTextBrushKey;
                case SystemBrushKey.InactiveSelectionHighlight:
                    return SystemColors.InactiveSelectionHighlightBrushKey;
                case SystemBrushKey.InactiveSelectionHighlightText:
                    return SystemColors.InactiveSelectionHighlightTextBrushKey;
                default:
                    throw new ArgumentException("Unexpected colorKey value: {0}".Fmt(colorKey));
            }
        }
    }

    public enum SystemBrushKey
    {
        ActiveBorder,
        ActiveCaption,
        ActiveCaptionText,
        AppWorkspace,
        Control,
        ControlDark,
        ControlDarkDark,
        ControlLight,
        ControlLightLight,
        ControlText,
        Desktop,
        GradientActiveCaption,
        GradientInactiveCaption,
        GrayText,
        Highlight,
        HighlightText,
        HotTrack,
        InactiveBorder,
        InactiveCaption,
        InactiveCaptionText,
        Info,
        InfoText,
        Menu,
        MenuBar,
        MenuHighlight,
        MenuText,
        ScrollBar,
        Window,
        WindowFrame,
        WindowText,
        InactiveSelectionHighlight,
        InactiveSelectionHighlightText,
    }
}
