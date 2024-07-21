using System.ComponentModel;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiMenuItemSeparator : Separator, IStiWpfV2Control
    {
        public StiMenuItemSeparator()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiMenuItemSeparator;
        }
    }
}
