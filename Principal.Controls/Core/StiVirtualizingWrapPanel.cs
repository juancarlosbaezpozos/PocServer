using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiVirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        private TranslateTransform trans = new TranslateTransform();

        private Size extent = new Size(0.0, 0.0);

        private Size viewport = new Size(0.0, 0.0);

        private Point offset;

        public static readonly DependencyProperty ScrollOffsetProperty = DependencyProperty.RegisterAttached("ScrollOffset", typeof(int), typeof(StiVirtualizingWrapPanel), new PropertyMetadata(0));

        public int ChildWidth { get; set; } = 30;


        public int ChildHeight { get; set; } = 30;


        public ScrollViewer ScrollOwner { get; set; }

        public bool CanHorizontallyScroll { get; set; }

        public bool CanVerticallyScroll { get; set; }

        public double HorizontalOffset => offset.X;

        public double VerticalOffset => offset.Y;

        public double ExtentHeight => extent.Height;

        public double ExtentWidth => extent.Width;

        public double ViewportHeight => viewport.Height;

        public double ViewportWidth => viewport.Width;

        public int ScrollOffset
        {
            get
            {
                return Convert.ToInt32(GetValue(ScrollOffsetProperty));
            }
            set
            {
                SetValue(ScrollOffsetProperty, value);
            }
        }

        public StiVirtualizingWrapPanel()
        {
            base.RenderTransform = trans;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            try
            {
                UpdateScrollInfo(availableSize);
                int firstVisibleItemIndex = 0;
                int lastVisibleItemIndex = 0;
                GetVisibleRange(ref firstVisibleItemIndex, ref lastVisibleItemIndex);
                UIElementCollection internalChildren = base.InternalChildren;
                IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
                GeneratorPosition position = itemContainerGenerator.GeneratorPositionFromIndex(firstVisibleItemIndex);
                int num = ((position.Offset == 0) ? position.Index : (position.Index + 1));
                using (itemContainerGenerator.StartAt(position, GeneratorDirection.Forward, allowStartAtRealizedItem: true))
                {
                    int num2 = firstVisibleItemIndex;
                    while (num2 <= lastVisibleItemIndex)
                    {
                        UIElement uIElement = itemContainerGenerator.GenerateNext(out var isNewlyRealized) as UIElement;
                        if (isNewlyRealized)
                        {
                            if (num >= internalChildren.Count)
                            {
                                AddInternalChild(uIElement);
                            }
                            else
                            {
                                InsertInternalChild(num, uIElement);
                            }
                            itemContainerGenerator.PrepareItemContainer(uIElement);
                        }
                        uIElement.Measure(GetChildSize());
                        num2++;
                        num++;
                    }
                }
                CleanUpItems(firstVisibleItemIndex, lastVisibleItemIndex);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (NullReferenceException)
            {
            }
            return new Size(double.IsInfinity(availableSize.Width) ? 0.0 : availableSize.Width, double.IsInfinity(availableSize.Height) ? 0.0 : availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
            UpdateScrollInfo(finalSize);
            for (int i = 0; i <= base.Children.Count - 1; i++)
            {
                UIElement child = base.Children[i];
                int itemIndex = itemContainerGenerator.IndexFromGeneratorPosition(new GeneratorPosition(i, 0));
                ArrangeChild(itemIndex, child, finalSize);
            }
            return finalSize;
        }

        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
                    break;
                case NotifyCollectionChangedAction.Move:
                    RemoveInternalChildRange(args.OldPosition.Index, args.ItemUICount);
                    break;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetVerticalOffset(VerticalOffset);
        }

        protected override void OnClearChildren()
        {
            base.OnClearChildren();
            SetVerticalOffset(0.0);
        }

        protected override void BringIndexIntoView(int index)
        {
            if (index >= 0 && index < base.Children.Count)
            {
                int num = CalculateChildrenPerRow(base.RenderSize);
                int num2 = index / num;
                SetVerticalOffset(num2 * ChildHeight);
            }
        }

        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            UIElementCollection internalChildren = base.InternalChildren;
            IItemContainerGenerator itemContainerGenerator = base.ItemContainerGenerator;
            int itemCount = GetItemCount();
            int num = CalculateChildrenPerRow(extent);
            if (minDesiredGenerated - 2 * num > 0)
            {
                minDesiredGenerated -= 2 * num;
            }
            if (maxDesiredGenerated + 2 * num < itemCount)
            {
                maxDesiredGenerated += 2 * num;
            }
            for (int i = internalChildren.Count - 1; i >= 0; i += -1)
            {
                GeneratorPosition position = new GeneratorPosition(i, 0);
                int num2 = itemContainerGenerator.IndexFromGeneratorPosition(position);
                if ((num2 > 2 * num - 1 && num2 < minDesiredGenerated) || (num2 < itemCount - 2 * num - 1 && num2 > maxDesiredGenerated))
                {
                    itemContainerGenerator.Remove(position, 1);
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private Size CalculateExtent(Size availableSize, int itemCount)
        {
            int num = CalculateChildrenPerRow(availableSize);
            return new Size(num * ChildWidth, (double)ChildHeight * Math.Ceiling(Convert.ToDouble(itemCount) / (double)num));
        }

        private void GetVisibleRange(ref int firstVisibleItemIndex, ref int lastVisibleItemIndex)
        {
            if (double.IsInfinity(viewport.Height))
            {
                return;
            }
            int num = CalculateChildrenPerRow(extent);
            try
            {
                firstVisibleItemIndex = Convert.ToInt32(Math.Floor(offset.Y / (double)ChildHeight)) * num;
                lastVisibleItemIndex = Convert.ToInt32(Math.Ceiling((offset.Y + viewport.Height) / (double)ChildHeight)) * num - 1;
                int itemCount = GetItemCount();
                if (lastVisibleItemIndex >= itemCount)
                {
                    lastVisibleItemIndex = itemCount - 1;
                }
            }
            catch
            {
            }
        }

        private Size GetChildSize()
        {
            return new Size(ChildWidth, ChildHeight);
        }

        private void ArrangeChild(int itemIndex, UIElement child, Size finalSize)
        {
            int num = CalculateChildrenPerRow(finalSize);
            int num2 = itemIndex / num;
            int num3 = itemIndex % num;
            double x = num3 * ChildWidth;
            child.Arrange(new Rect(x, num2 * ChildHeight, ChildWidth, ChildHeight));
        }

        private int CalculateChildrenPerRow(Size availableSize)
        {
            if (availableSize.Width != double.PositiveInfinity)
            {
                return Math.Max(1, (int)(availableSize.Width / (double)ChildWidth));
            }
            return base.Children.Count;
        }

        private int GetItemCount()
        {
            ItemsControl itemsOwner = ItemsControl.GetItemsOwner(this);
            if (!itemsOwner.HasItems)
            {
                return 0;
            }
            return itemsOwner.Items.Count;
        }

        private void UpdateScrollInfo(Size availableSize)
        {
            int itemCount = GetItemCount();
            Size size = CalculateExtent(availableSize, itemCount);
            if (size != extent)
            {
                extent = size;
                if (ScrollOwner != null)
                {
                    ScrollOwner.InvalidateScrollInfo();
                }
            }
            if (availableSize != viewport)
            {
                viewport = availableSize;
                if (ScrollOwner != null)
                {
                    ScrollOwner.InvalidateScrollInfo();
                }
            }
        }

        public void LineUp()
        {
            SetVerticalOffset(VerticalOffset - (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
        }

        public void LineDown()
        {
            SetVerticalOffset(VerticalOffset + (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - viewport.Height);
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + viewport.Height);
        }

        public void MouseWheelUp()
        {
            EasingSetVerticalOffset(VerticalOffset - (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
        }

        public void MouseWheelDown()
        {
            EasingSetVerticalOffset(VerticalOffset + (double)((ScrollOffset > 0) ? ScrollOffset : ChildHeight));
        }

        public void LineLeft()
        {
            throw new InvalidOperationException();
        }

        public void LineRight()
        {
            throw new InvalidOperationException();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return default(Rect);
        }

        public void MouseWheelLeft()
        {
            throw new InvalidOperationException();
        }

        public void MouseWheelRight()
        {
            throw new InvalidOperationException();
        }

        public void PageLeft()
        {
            throw new InvalidOperationException();
        }

        public void PageRight()
        {
            throw new InvalidOperationException();
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new InvalidOperationException();
        }

        public void EasingSetVerticalOffset(double offset)
        {
            AdjustVerticalOffset(ref offset);
            trans.Y = 0.0 - offset;
            InvalidateMeasure();
        }

        public void SetVerticalOffset(double offset)
        {
            AdjustVerticalOffset(ref offset);
            trans.Y = 0.0 - offset;
            InvalidateMeasure();
        }

        private void AdjustVerticalOffset(ref double offset)
        {
            if (offset < 0.0 || viewport.Height >= extent.Height)
            {
                offset = 0.0;
            }
            else if (offset + viewport.Height >= extent.Height)
            {
                offset = extent.Height - viewport.Height;
            }
            this.offset.Y = offset;
            ScrollOwner?.InvalidateScrollInfo();
        }
    }
}
