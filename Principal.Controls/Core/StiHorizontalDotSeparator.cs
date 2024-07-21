using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class StiHorizontalDotSeparator : Control, IStiWpfV2Control
    {
        public static readonly DependencyProperty FillSeparatorProperty = DependencyProperty.Register("FillSeparator", typeof(Brush), typeof(StiHorizontalDotSeparator));

        public StiHorizontalDotSeparator()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                ImageBrush value = new ImageBrush(StiReportWpfImages.CloudWindow.DotSeparator().ToReportWpfBitmapImage())
                {
                    Stretch = ((StiScale.Id != 0) ? Stretch.Uniform : Stretch.None),
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0.0, 0.0, 99.0, 1.0),
                    ViewportUnits = BrushMappingMode.Absolute,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top
                };
                SetValue(FillSeparatorProperty, value);
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiHorizontalDotSeparator;
        }
    }
}
