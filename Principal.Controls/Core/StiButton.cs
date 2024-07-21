using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiButton : Button, IStiWpfV2Control
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(StiButton), new PropertyMetadata(new CornerRadius(0.0)));

        public bool ApplyStyleInToolbar { get; set; } = true;

        public StiButton()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                if (StiOSVersion.IsWindows11())
                {
                    base.BorderThickness = new Thickness(StiScale.ScaleThickness);
                    SetValue(CornerRadiusProperty, new CornerRadius(2.0));
                }
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiButton;
        }
    }
}
