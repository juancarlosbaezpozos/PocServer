using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class ErrorMessageWhiteControl : ErrorMessageControl
    {
        public static readonly DependencyProperty InformationIconProperty = DependencyProperty.Register("InformationIcon", typeof(BitmapImage), typeof(ErrorMessageWhiteControl));

        public ErrorMessageWhiteControl()
        {
            SetValue(InformationIconProperty, StiReportWpfImages.MessageBox.Information().ToReportWpfBitmapImage());
        }

        public override StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.ErrorMessageWhiteControl;
        }

        protected override BitmapImage GetIcon()
        {
            return StiReportWpfImages.ErrorMessageControl.Information().ToReportWpfBitmapImage();
        }
    }
}
