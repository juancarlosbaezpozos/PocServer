using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class BigProgressControl : Control, IStiWpfV2Control
    {
        private static BitmapImage source;

        public static readonly DependencyProperty BitmapProperty = DependencyProperty.Register("Bitmap", typeof(BitmapImage), typeof(BigProgressControl));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(BigProgressControl));

        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public BigProgressControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                if (source == null)
                {
                    source = StiReportWpfImages.BigProgressMarker.GetByTheme(StiWpfSkinHelper.CurrentForeground.ToString()).ToReportWpfBitmapImage();
                    StiWpfSkinHelper.SkinChanged += ThemeHelper_SkinChanged;
                }
                SetValue(BitmapProperty, source);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.BigProgressControl;
        }

        private static void ThemeHelper_SkinChanged(object sender, EventArgs e)
        {
            source = StiReportWpfImages.BigProgressMarker.GetByTheme(StiWpfSkinHelper.CurrentForeground.ToString()).ToReportWpfBitmapImage();
        }
    }
}
