using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiRadioButton : RadioButton, IStiWpfV2Control
    {
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(StiRadioButton), new FrameworkPropertyMetadata(1.0));

        public StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Win11;

        public StiRadioButton()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
                if (StiOSVersion.IsWindows11())
                {
                    base.BorderThickness = new Thickness(StiScale.ScaleThickness);
                    SetValue(StrokeThicknessProperty, StiScale.ScaleThickness);
                }
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiRadioButton;
        }
    }
}
