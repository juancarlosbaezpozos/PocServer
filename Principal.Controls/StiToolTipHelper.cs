using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Principal.Controls
{
    public static class StiToolTipHelper
    {
        public static ToolTip Get(string text, PlacementMode placement = PlacementMode.Bottom)
        {
            return new ToolTip
            {
                Content = new TextBlock
                {
                    Text = text,
                    Margin = new Thickness(4.0),
                    TextWrapping = TextWrapping.WrapWithOverflow
                },
                Placement = placement
            };
        }

        public static ToolTip Get(string text, double verticalOffset, PlacementMode placement = PlacementMode.Bottom)
        {
            return new ToolTip
            {
                Content = new TextBlock
                {
                    Text = text,
                    Margin = new Thickness(4.0),
                    TextWrapping = TextWrapping.WrapWithOverflow
                },
                VerticalOffset = verticalOffset,
                Placement = placement
            };
        }

        public static ToolTip Get(string header, string body, int maxWidth)
        {
            var toolTip = new ToolTip
            {
                VerticalOffset = 5.0
            };
            if (body != null && header == null)
            {
                var content = new TextBlock
                {
                    Text = body,
                    MaxWidth = maxWidth,
                    Margin = new Thickness(4.0),
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                toolTip.Content = content;
                return toolTip;
            }
            var stackPanel = new StackPanel
            {
                IsHitTestVisible = false,
                Orientation = Orientation.Vertical,
                MaxWidth = maxWidth
            };
            if (header != null)
            {
                var element = new TextBlock
                {
                    Text = header,
                    Margin = new Thickness(4.0),
                    FontWeight = FontWeights.Bold
                };
                stackPanel.Children.Add(element);
            }
            if (body != null)
            {
                var element2 = new TextBlock
                {
                    Text = body,
                    Margin = new Thickness(4.0),
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };
                stackPanel.Children.Add(element2);
            }
            toolTip.Content = stackPanel;
            return toolTip;
        }
    }
}
