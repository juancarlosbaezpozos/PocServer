using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class CloudButtonSettings : ToggleButton, IStiWpfV2Control
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(CloudButtonSettings));

        public CloudButtonSettings()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                SetValue(IconProperty, StiReportWpfImages.Editor.Settings().ToReportWpfImage());
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.CloudButtonSettings;
        }
    }
}
