using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Principal.Controls
{
    public static class StiDropShadowHelper
    {
        private class Brushes
        {
            public readonly CornerRadius radius;

            public readonly Color color;

            public readonly Brush[] brushes;

            public Brushes(CornerRadius radius, Color color, Brush[] brushes)
            {
                this.radius = radius;
                this.color = color;
                this.brushes = brushes;
            }
        }

        private static List<Brushes> listOfBrushes;

        private static object lockAccess = new object();

        private static Brush[] GetBrushes(Color color, CornerRadius cornerRadius)
        {
            lock (lockAccess)
            {
                if (listOfBrushes == null)
                {
                    listOfBrushes = new List<Brushes>();
                }
                else
                {
                    foreach (var listOfBrush in listOfBrushes)
                    {
                        if (listOfBrush.color == color && listOfBrush.radius == cornerRadius)
                        {
                            return listOfBrush.brushes;
                        }
                    }
                }
                var array = CreateBrushes(color, cornerRadius);
                listOfBrushes.Add(new Brushes(cornerRadius, color, array));
                return array;
            }
        }

        private static Brush[] CreateBrushes(Color color, CornerRadius cornerRadius)
        {
            var array = new Brush[9];
            var gradientStopCollection = CreateStops(color, 0.0);
            array[4] = new SolidColorBrush(color);
            array[1] = new LinearGradientBrush(gradientStopCollection, new Point(0.0, 1.0), new Point(0.0, 0.0));
            array[3] = new LinearGradientBrush(gradientStopCollection, new Point(1.0, 0.0), new Point(0.0, 0.0));
            array[5] = new LinearGradientBrush(gradientStopCollection, new Point(0.0, 0.0), new Point(1.0, 0.0));
            array[7] = new LinearGradientBrush(gradientStopCollection, new Point(0.0, 0.0), new Point(0.0, 1.0));
            var gradientStopCollection2 = ((cornerRadius.TopLeft == 0.0) ? gradientStopCollection : CreateStops(color, cornerRadius.TopLeft));
            var radialGradientBrush = new RadialGradientBrush(gradientStopCollection2)
            {
                RadiusX = 1.0,
                RadiusY = 1.0,
                Center = new Point(1.0, 1.0),
                GradientOrigin = new Point(1.0, 1.0)
            };
            array[0] = radialGradientBrush;
            var gradientStopCollection3 = ((cornerRadius.TopRight == 0.0) ? gradientStopCollection : ((cornerRadius.TopRight != cornerRadius.TopLeft) ? CreateStops(color, cornerRadius.TopRight) : gradientStopCollection2));
            var radialGradientBrush2 = new RadialGradientBrush(gradientStopCollection3)
            {
                RadiusX = 1.0,
                RadiusY = 1.0,
                Center = new Point(0.0, 1.0),
                GradientOrigin = new Point(0.0, 1.0)
            };
            array[2] = radialGradientBrush2;
            var gradientStopCollection4 = ((cornerRadius.BottomLeft == 0.0) ? gradientStopCollection : ((cornerRadius.BottomLeft == cornerRadius.TopLeft) ? gradientStopCollection2 : ((cornerRadius.BottomLeft != cornerRadius.TopRight) ? CreateStops(color, cornerRadius.BottomLeft) : gradientStopCollection3)));
            var radialGradientBrush3 = new RadialGradientBrush(gradientStopCollection4)
            {
                RadiusX = 1.0,
                RadiusY = 1.0,
                Center = new Point(1.0, 0.0),
                GradientOrigin = new Point(1.0, 0.0)
            };
            array[6] = radialGradientBrush3;
            var gradientStopCollection5 = ((cornerRadius.BottomRight == 0.0) ? gradientStopCollection : ((cornerRadius.BottomRight == cornerRadius.TopLeft) ? gradientStopCollection2 : ((cornerRadius.BottomRight == cornerRadius.TopRight) ? gradientStopCollection3 : ((cornerRadius.BottomRight != cornerRadius.BottomLeft) ? CreateStops(color, cornerRadius.BottomRight) : gradientStopCollection4))));
            var radialGradientBrush4 = new RadialGradientBrush(gradientStopCollection5)
            {
                RadiusX = 1.0,
                RadiusY = 1.0,
                Center = new Point(0.0, 0.0),
                GradientOrigin = new Point(0.0, 0.0)
            };
            array[8] = radialGradientBrush4;
            return array;
        }

        private static GradientStopCollection CreateStops(Color color, double cornerRadius)
        {
            var num = 1.0 / (cornerRadius + 5.0);
            var gradientStopCollection = new GradientStopCollection
        {
            new GradientStop(color, (0.5 + cornerRadius) * num)
        };
            var color2 = color;
            color2.A = (byte)(0.74336 * (double)(int)color.A);
            gradientStopCollection.Add(new GradientStop(color, (1.5 + cornerRadius) * num));
            color2.A = (byte)(0.38053 * (double)(int)color.A);
            gradientStopCollection.Add(new GradientStop(color2, (2.5 + cornerRadius) * num));
            color2.A = (byte)(0.12389 * (double)(int)color.A);
            gradientStopCollection.Add(new GradientStop(color2, (3.5 + cornerRadius) * num));
            color2.A = (byte)(0.02654 * (double)(int)color.A);
            gradientStopCollection.Add(new GradientStop(color2, (4.5 + cornerRadius) * num));
            color2.A = 0;
            gradientStopCollection.Add(new GradientStop(color2, (5.0 + cornerRadius) * num));
            return gradientStopCollection;
        }

        public static void DrawShadow(DrawingContext dc, CornerRadius cornerRadius, Rect rect, Color color)
        {
            DrawShadow(dc, cornerRadius, rect, color, isBottom: false);
        }

        public static void DrawShadow(DrawingContext dc, CornerRadius cornerRadius, Rect rect, Color color, bool isBottom)
        {
            if (!(rect.Width > 0.0) || !(rect.Height > 0.0) || color.A <= 0)
            {
                return;
            }
            var num = rect.Right - rect.Left - 10.0;
            var num2 = rect.Bottom - rect.Top - 10.0;
            var val = Math.Min(num * 0.5, num2 * 0.5);
            cornerRadius.TopLeft = Math.Min(cornerRadius.TopLeft, val);
            cornerRadius.TopRight = Math.Min(cornerRadius.TopRight, val);
            cornerRadius.BottomLeft = Math.Min(cornerRadius.BottomLeft, val);
            cornerRadius.BottomRight = Math.Min(cornerRadius.BottomRight, val);
            var brushes = GetBrushes(color, cornerRadius);
            var num3 = rect.Top + 5.0;
            var num4 = rect.Left + 5.0;
            var num5 = rect.Right - 5.0;
            var num6 = rect.Bottom - 5.0;
            var array = new double[6]
            {
            num4,
            num4 + cornerRadius.TopLeft,
            num5 - cornerRadius.TopRight,
            num4 + cornerRadius.BottomLeft,
            num5 - cornerRadius.BottomRight,
            num5
            };
            var array2 = new double[6]
            {
            num3,
            num3 + cornerRadius.TopLeft,
            num3 + cornerRadius.TopRight,
            num6 - cornerRadius.BottomLeft,
            num6 - cornerRadius.BottomRight,
            num6
            };
            dc.PushGuidelineSet(new GuidelineSet(array, array2));
            cornerRadius.TopLeft += 5.0;
            cornerRadius.TopRight += 5.0;
            cornerRadius.BottomLeft += 5.0;
            cornerRadius.BottomRight += 5.0;
            var rectangle = new Rect(rect.Left, rect.Top, cornerRadius.TopLeft, cornerRadius.TopLeft);
            if (!isBottom)
            {
                dc.DrawRectangle(brushes[0], null, rectangle);
            }
            var num7 = array[2] - array[1];
            if (num7 > 0.0)
            {
                rectangle = new Rect(array[1], rect.Top, num7, 5.0);
                if (!isBottom)
                {
                    dc.DrawRectangle(brushes[1], null, rectangle);
                }
            }
            rectangle = new Rect(array[2], rect.Top, cornerRadius.TopRight, cornerRadius.TopRight);
            if (!isBottom)
            {
                dc.DrawRectangle(brushes[2], null, rectangle);
            }
            num7 = array2[3] - array2[1];
            if (num7 > 0.0)
            {
                rectangle = new Rect(rect.Left, array2[1], 5.0, num7);
                if (!isBottom)
                {
                    dc.DrawRectangle(brushes[3], null, rectangle);
                }
            }
            num7 = array2[4] - array2[2];
            if (num7 > 0.0)
            {
                rectangle = new Rect(array[5], array2[2], 5.0, num7);
                if (!isBottom)
                {
                    dc.DrawRectangle(brushes[5], null, rectangle);
                }
            }
            rectangle = new Rect(rect.Left, array2[3], cornerRadius.BottomLeft, cornerRadius.BottomLeft);
            if (!isBottom)
            {
                dc.DrawRectangle(brushes[6], null, rectangle);
            }
            num7 = array[4] - array[3];
            if (num7 > 0.0)
            {
                dc.DrawRectangle(rectangle: new Rect(array[3], array2[5], num7, 5.0), brush: brushes[7], pen: null);
            }
            rectangle = new Rect(array[4], array2[4], cornerRadius.BottomRight, cornerRadius.BottomRight);
            if (!isBottom)
            {
                dc.DrawRectangle(brushes[8], null, rectangle);
            }
            if (cornerRadius.TopLeft == 5.0 && cornerRadius.TopLeft == cornerRadius.TopRight && cornerRadius.TopLeft == cornerRadius.BottomLeft && cornerRadius.TopLeft == cornerRadius.BottomRight)
            {
                rectangle = new Rect(array[0], array2[0], num, num2);
                if (!isBottom)
                {
                    dc.DrawRectangle(brushes[4], null, rectangle);
                }
            }
            else
            {
                var pathFigure = new PathFigure();
                if (cornerRadius.TopLeft > 5.0)
                {
                    pathFigure.StartPoint = new Point(array[1], array2[0]);
                    pathFigure.Segments.Add(new LineSegment(new Point(array[1], array2[1]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[0], array2[1]), isStroked: true));
                }
                else
                {
                    pathFigure.StartPoint = new Point(array[0], array2[0]);
                }
                if (cornerRadius.BottomLeft > 5.0)
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[0], array2[3]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[3], array2[3]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[3], array2[5]), isStroked: true));
                }
                else
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[0], array2[5]), isStroked: true));
                }
                if (cornerRadius.BottomRight > 5.0)
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[4], array2[5]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[4], array2[4]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[5], array2[4]), isStroked: true));
                }
                else
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[5], array2[5]), isStroked: true));
                }
                if (cornerRadius.TopRight > 5.0)
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[5], array2[2]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[2], array2[2]), isStroked: true));
                    pathFigure.Segments.Add(new LineSegment(new Point(array[2], array2[0]), isStroked: true));
                }
                else
                {
                    pathFigure.Segments.Add(new LineSegment(new Point(array[5], array2[0]), isStroked: true));
                }
                pathFigure.IsClosed = true;
                var pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);
                if (!isBottom)
                {
                    dc.DrawGeometry(brushes[4], null, pathGeometry);
                }
            }
            dc.Pop();
        }
    }
}
