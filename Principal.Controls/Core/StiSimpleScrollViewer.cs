using System.ComponentModel;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class StiSimpleScrollViewer : ScrollViewer, IStiWpfV2Control
    {
        public StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Win11;

        public StiSimpleScrollViewer()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiSimpleScrollViewer;
        }
    }
}
