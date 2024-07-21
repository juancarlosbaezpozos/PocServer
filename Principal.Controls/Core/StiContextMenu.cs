using System.ComponentModel;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiContextMenu : ContextMenu, IStiWpfV2Control
    {
        public StiContextMenu()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiContextMenu;
        }
    }
}
