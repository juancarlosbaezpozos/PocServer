using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class ErrorMessageControl : Control, IStiWpfV2Control
    {
        private Popup popupWindow;

        private double LeftPos;

        private double WidthValue;

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(ErrorMessageControl));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ErrorMessageControl));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(ErrorMessageControl), new FrameworkPropertyMetadata(TextAlignment.Center));

        public static readonly DependencyProperty ShowMarkerProperty = DependencyProperty.Register("ShowMarker", typeof(Visibility), typeof(ErrorMessageControl), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ShowArrowProperty = DependencyProperty.Register("ShowArrow", typeof(bool), typeof(ErrorMessageControl), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ErrorAlignmentProperty = DependencyProperty.Register("ErrorAlignment", typeof(StiErrorAlignment), typeof(ErrorMessageControl), new FrameworkPropertyMetadata(StiErrorAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public object Icon
        {
            get
            {
                return GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return GetValue(TextProperty) as string;
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                return (TextAlignment)GetValue(TextAlignmentProperty);
            }
            set
            {
                SetValue(TextAlignmentProperty, value);
            }
        }

        public Visibility ShowMarker
        {
            get
            {
                return (Visibility)GetValue(ShowMarkerProperty);
            }
            set
            {
                SetValue(ShowMarkerProperty, value);
            }
        }

        public bool ShowArrow
        {
            get
            {
                return (bool)GetValue(ShowArrowProperty);
            }
            set
            {
                SetValue(ShowArrowProperty, value);
            }
        }

        public StiErrorAlignment ErrorAlignment
        {
            get
            {
                return (StiErrorAlignment)GetValue(ErrorAlignmentProperty);
            }
            set
            {
                SetValue(ErrorAlignmentProperty, value);
            }
        }

        public ErrorMessageControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Image value = new Image
                {
                    Width = 32.0,
                    Height = 32.0,
                    Stretch = Stretch.Uniform,
                    Source = GetIcon(),
                    IsHitTestVisible = false
                };
                SetValue(IconProperty, value);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.ErrorMessageControl;
        }

        protected virtual BitmapImage GetIcon()
        {
            return StiReportWpfImages.ErrorMessageControl.WarningWhite().ToReportWpfBitmapImage();
        }

        internal void AutoSize(Popup popupWindow, double leftPos, double widthValue)
        {
            this.popupWindow = popupWindow;
            LeftPos = leftPos;
            WidthValue = widthValue;
            base.SizeChanged += ThisSizeChanged;
        }

        private void ThisSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ErrorAlignment == StiErrorAlignment.Bottom)
            {
                popupWindow.HorizontalOffset = LeftPos + (WidthValue - e.NewSize.Width) / 2.0;
            }
            else
            {
                popupWindow.HorizontalOffset = LeftPos - e.NewSize.Width;
            }
        }
    }
}
