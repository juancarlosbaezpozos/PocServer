using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class ProgressControl : Control, IStiWpfV2Control
    {
        private static BitmapImage source;

        public static readonly DependencyProperty BitmapProperty = DependencyProperty.Register("Bitmap", typeof(BitmapImage), typeof(ProgressControl));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(ProgressControl));

        public BitmapImage Bitmap
        {
            get
            {
                return (BitmapImage)GetValue(BitmapProperty);
            }
            set
            {
                SetValue(BitmapProperty, value);
            }
        }

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

        public ProgressControl()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            StiWpfV2ThemeHelper.LoadControlTheme(this);
            if (source == null)
            {
                StiSkinForeground stiSkinForeground = StiWpfSkinHelper.CurrentForeground;
                if (StiWpfSkinHelper.IsWinFormsMode)
                {
                    stiSkinForeground = StiSkinForeground.Blue;
                }
                source = StiReportWpfImages.ProgressMarker.GetByTheme(stiSkinForeground.ToString()).ToReportWpfBitmapImage();
                StiWpfSkinHelper.SkinChanged += ThemeHelper_SkinChanged;
            }
            SetValue(BitmapProperty, source);
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.ProgressControl;
        }

        private static void ThemeHelper_SkinChanged(object sender, EventArgs e)
        {
            source = StiReportWpfImages.ProgressMarker.GetByTheme(StiWpfSkinHelper.CurrentForeground.ToString()).ToReportWpfBitmapImage();
        }
    }
}
