using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class StiDropShadowDecorator : Decorator
    {
        private Color color = Color.FromArgb(121, 0, 0, 0);

        private CornerRadius cornerRadius;

        private StiOffsetMode offsetMode;

        private double offset = 5.0;

        private Point offsetLocation = new Point(0.0, 0.0);

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return cornerRadius;
            }
            set
            {
                cornerRadius = value;
            }
        }

        public StiOffsetMode OffsetMode
        {
            get
            {
                return offsetMode;
            }
            set
            {
                offsetMode = value;
            }
        }

        public double Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
            }
        }

        public Point OffsetLocation
        {
            get
            {
                return offsetLocation;
            }
            set
            {
                offsetLocation = value;
            }
        }

        public StiDropShadowDecorator()
        {
            base.Margin = new Thickness(5.0);
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (OffsetMode == StiOffsetMode.Shift)
            {
                Rect rect = new Rect(new Point(offset + offsetLocation.X, offset + offsetLocation.Y), new Size(base.RenderSize.Width, base.RenderSize.Height));
                StiDropShadowHelper.DrawShadow(dc, cornerRadius, rect, color);
            }
            else if (offsetMode == StiOffsetMode.Bottom)
            {
                if (cornerRadius.BottomLeft == 0.0 && cornerRadius.BottomRight == 0.0 && cornerRadius.TopLeft == 0.0 && cornerRadius.TopRight == 0.0)
                {
                    Rect rect2 = new Rect(new Point(offsetLocation.X, offset + offsetLocation.Y), new Size(base.RenderSize.Width, base.RenderSize.Height));
                    rect2.Inflate(offset, 0.0);
                    StiDropShadowHelper.DrawShadow(dc, cornerRadius, rect2, color);
                }
                else
                {
                    Rect rect3 = new Rect(new Point(offsetLocation.X, offset + offsetLocation.Y), new Size(base.RenderSize.Width, base.RenderSize.Height));
                    StiDropShadowHelper.DrawShadow(dc, cornerRadius, rect3, color);
                }
            }
            else
            {
                Rect rect4 = new Rect(new Point((0.0 - offset) / 2.0 + offsetLocation.X, (0.0 - offset) / 2.0 + offsetLocation.Y), new Size(base.RenderSize.Width + offset, base.RenderSize.Height + offset));
                StiDropShadowHelper.DrawShadow(dc, cornerRadius, rect4, color);
            }
        }
    }
}
