using System;

namespace Principal.Controls
{
    public static class StiWpfSkinHelper
    {
        internal static bool IsWinFormsMode { get; set; }

        public static StiSkinBackground CurrentBackground { get; private set; }

        public static StiSkinForeground CurrentForeground { get; private set; }

        public static event EventHandler SkinChanged;

        static StiWpfSkinHelper()
        {
            CurrentBackground = StiSkinBackground.White;
            CurrentForeground = StiSkinForeground.Blue;
        }

        public static void SetNewSkin(StiSkinBackground background, StiSkinForeground foreground)
        {
            CurrentBackground = background;
            CurrentForeground = foreground;
            StiWpfSkinHelper.SkinChanged?.Invoke(null, null);
        }
    }
}
