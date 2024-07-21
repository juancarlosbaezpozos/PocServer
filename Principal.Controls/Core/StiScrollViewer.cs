using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiScrollViewer : ScrollViewer, IStiWpfV2Control
    {
        public virtual StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Win11;

        public StiScrollViewer()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
                if (StiOSVersion.IsWindows11())
                {
                    base.BorderThickness = new Thickness(StiScale.ScaleThickness);
                }
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiScrollViewer;
        }
    }
}
