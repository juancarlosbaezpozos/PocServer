using System.ComponentModel;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class CloudVerticalTabControl : TabControl, IStiWpfV2Control
    {
        public CloudVerticalTabControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.CloudVerticalTabControl;
        }
    }
}
