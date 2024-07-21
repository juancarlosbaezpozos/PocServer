using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiComboBox : ComboBox, IStiWpfV2Control
    {
        public virtual StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Default;

        public StiComboBox()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
                base.BorderThickness = StiControlsV2Win11Helper.Thickness1111;
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiComboBox;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StiComboBoxItem();
        }
    }
}
