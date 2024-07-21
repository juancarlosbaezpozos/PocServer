using Principal.Server.Objects;
using System.Windows;

namespace Principal.Controls.Core
{
    public static class StiControlsV2Win11Helper
    {
        public static double SeparatorLine
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return 1.0;
                }
                return StiScale.ScaleThickness;
            }
        }

        public static double DefaultRadius => StiOSVersion.IsWindows11() ? 2 : 0;

        public static CornerRadius RibbonButtonCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(3.0);
            }
        }

        public static CornerRadius RibbonButtonCornerRadius0330
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(0.0, 3.0, 3.0, 0.0);
            }
        }

        public static CornerRadius ButtonCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(2.0);
            }
        }

        public static CornerRadius ButtonEventPG2CornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(0.0, 2.0, 2.0, 0.0);
            }
        }

        public static CornerRadius WindowsCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(9.0);
            }
        }

        public static CornerRadius WindowsFooterCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(0.0, 0.0, 9.0, 9.0);
            }
        }

        public static CornerRadius DesignerPanelCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(0.0, 0.0, 10.0, 10.0);
            }
        }

        public static CornerRadius CloseButtonCornerRadius
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new CornerRadius(0.0);
                }
                return new CornerRadius(0.0, 9.0, 0.0, 0.0);
            }
        }

        public static Thickness Thickness1111
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0);
                }
                return new Thickness(StiScale.ScaleThickness);
            }
        }

        public static Thickness Thickness0100
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 1.0, 0.0, 0.0);
                }
                return new Thickness(0.0, StiScale.ScaleThickness, 0.0, 0.0);
            }
        }

        public static Thickness Thickness1100
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0, 1.0, 0.0, 0.0);
                }
                return new Thickness(StiScale.ScaleThickness, StiScale.ScaleThickness, 0.0, 0.0);
            }
        }

        public static Thickness Thickness1010
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0, 0.0, 1.0, 0.0);
                }
                return new Thickness(StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness, 0.0);
            }
        }

        public static Thickness Thickness0110
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 1.0, 1.0, 0.0);
                }
                return new Thickness(0.0, StiScale.ScaleThickness, StiScale.ScaleThickness, 0.0);
            }
        }

        public static Thickness Thickness1000
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0, 0.0, 0.0, 0.0);
                }
                return new Thickness(StiScale.ScaleThickness, 0.0, 0.0, 0.0);
            }
        }

        public static Thickness Thickness0001
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 0.0, 0.0, 1.0);
                }
                return new Thickness(0.0, 0.0, 0.0, StiScale.ScaleThickness);
            }
        }

        public static Thickness Thickness44030
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(4.0, 4.0, 0.0, 30.0);
                }
                return new Thickness(4.0, 4.0, 0.0, 30.0 + StiScale.ScaleThickness);
            }
        }

        public static Thickness Thickness1011
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0, 0.0, 1.0, 1.0);
                }
                return new Thickness(StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness, StiScale.ScaleThickness);
            }
        }

        public static Thickness Thickness1110
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(1.0, 1.0, 1.0, 0.0);
                }
                return new Thickness(StiScale.ScaleThickness, StiScale.ScaleThickness, StiScale.ScaleThickness, 0.0);
            }
        }

        public static Thickness DockingPanelItemLeftRect
        {
            get
            {
                if (StiScale.Factor != 1.0)
                {
                    return new Thickness(-3.0 + StiScale.ScaleThickness, StiScale.ScaleThickness, 0.0, 0.0);
                }
                return new Thickness(-2.0, 0.0, 0.0, 0.0);
            }
        }

        public static Thickness DockingPanelItemRightRect
        {
            get
            {
                if (StiScale.Factor != 1.0)
                {
                    return new Thickness(0.0, StiScale.ScaleThickness, -3.0 + StiScale.ScaleThickness, 0.0);
                }
                return new Thickness(0.0, 0.0, -2.0, 0.0);
            }
        }

        public static Thickness RibbonPageButtonV2RightRect
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 0.0, -3.0, 0.0);
                }
                return new Thickness(0.0, 0.0, -3.0 + StiScale.ScaleThickness, 0.0);
            }
        }

        public static Thickness RibbonPageButtonV2Margins
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, -1.0, 0.0, 0.0);
                }
                return new Thickness(0.0, 0.0 - StiScale.ScaleThickness, 0.0, 0.0);
            }
        }

        public static Thickness ExpanderHeaderMargins
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 0.0, 0.0, 0.0);
                }
                return new Thickness(StiScale.ScaleThickness, 0.0, StiScale.ScaleThickness, 0.0);
            }
        }

        public static Thickness DockingItemContainerV2HeaderMargins
        {
            get
            {
                if (!StiOSVersion.IsWindows11())
                {
                    return new Thickness(0.0, 0.0, 0.0, 0.0);
                }
                return new Thickness(0.0 - StiScale.ScaleThickness, 0.0 - StiScale.ScaleThickness, 0.0 - StiScale.ScaleThickness, 0.0);
            }
        }
    }
}
