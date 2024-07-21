using System.ComponentModel;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiTextBox : TextBox, IStiWpfV2Control
    {
        public virtual StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Default;

        public StiTextBox()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiTextBox;
        }
    }
}
