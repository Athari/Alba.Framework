using System;
using System.Windows;
using Alba.Framework.Text;

namespace Alba.Framework.Markup
{
    public class SystemColorExtension : DynamicResourceExtension
    {
        public SystemColorExtension (SystemColorKey colorKey)
            : base(GetColorResourceKey(colorKey))
        {}

        private static object GetColorResourceKey (SystemColorKey colorKey)
        {
            switch (colorKey) {
                case SystemColorKey.ActiveBorder:
                    return SystemColors.ActiveBorderColorKey;
                case SystemColorKey.ActiveCaption:
                    return SystemColors.ActiveCaptionColorKey;
                case SystemColorKey.ActiveCaptionText:
                    return SystemColors.ActiveCaptionTextColorKey;
                case SystemColorKey.AppWorkspace:
                    return SystemColors.AppWorkspaceColorKey;
                case SystemColorKey.Control:
                    return SystemColors.ControlColorKey;
                case SystemColorKey.ControlDark:
                    return SystemColors.ControlDarkColorKey;
                case SystemColorKey.ControlDarkDark:
                    return SystemColors.ControlDarkDarkColorKey;
                case SystemColorKey.ControlLight:
                    return SystemColors.ControlLightColorKey;
                case SystemColorKey.ControlLightLight:
                    return SystemColors.ControlLightLightColorKey;
                case SystemColorKey.ControlText:
                    return SystemColors.ControlTextColorKey;
                case SystemColorKey.Desktop:
                    return SystemColors.DesktopColorKey;
                case SystemColorKey.GradientActiveCaption:
                    return SystemColors.GradientActiveCaptionColorKey;
                case SystemColorKey.GradientInactiveCaption:
                    return SystemColors.GradientInactiveCaptionColorKey;
                case SystemColorKey.GrayText:
                    return SystemColors.GrayTextColorKey;
                case SystemColorKey.Highlight:
                    return SystemColors.HighlightColorKey;
                case SystemColorKey.HighlightText:
                    return SystemColors.HighlightTextColorKey;
                case SystemColorKey.HotTrack:
                    return SystemColors.HotTrackColorKey;
                case SystemColorKey.InactiveBorder:
                    return SystemColors.InactiveBorderColorKey;
                case SystemColorKey.InactiveCaption:
                    return SystemColors.InactiveCaptionColorKey;
                case SystemColorKey.InactiveCaptionText:
                    return SystemColors.InactiveCaptionTextColorKey;
                case SystemColorKey.Info:
                    return SystemColors.InfoColorKey;
                case SystemColorKey.InfoText:
                    return SystemColors.InfoTextColorKey;
                case SystemColorKey.Menu:
                    return SystemColors.MenuColorKey;
                case SystemColorKey.MenuBar:
                    return SystemColors.MenuBarColorKey;
                case SystemColorKey.MenuHighlight:
                    return SystemColors.MenuHighlightColorKey;
                case SystemColorKey.MenuText:
                    return SystemColors.MenuTextColorKey;
                case SystemColorKey.ScrollBar:
                    return SystemColors.ScrollBarColorKey;
                case SystemColorKey.Window:
                    return SystemColors.WindowColorKey;
                case SystemColorKey.WindowFrame:
                    return SystemColors.WindowFrameColorKey;
                case SystemColorKey.WindowText:
                    return SystemColors.WindowTextColorKey;
                default:
                    throw new ArgumentException("Unexpected colorKey value: {0}".Fmt(colorKey));
            }
        }
    }

    public enum SystemColorKey
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
    }
}
