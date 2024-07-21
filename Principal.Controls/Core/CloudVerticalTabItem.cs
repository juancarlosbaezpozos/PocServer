using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class CloudVerticalTabItem : TabItem, IStiWpfV2Control
    {
        public static readonly RoutedEvent SelectedEvent;

        public static readonly RoutedEvent UnSelectedEvent;

        public event RoutedEventHandler Selected
        {
            add
            {
                AddHandler(SelectedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedEvent, value);
            }
        }

        public event RoutedEventHandler UnSelected
        {
            add
            {
                AddHandler(UnSelectedEvent, value);
            }
            remove
            {
                RemoveHandler(UnSelectedEvent, value);
            }
        }

        public CloudVerticalTabItem()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.CloudVerticalTabItem;
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            RaiseEvent(new RoutedEventArgs(SelectedEvent));
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            RaiseEvent(new RoutedEventArgs(UnSelectedEvent));
        }

        static CloudVerticalTabItem()
        {
            SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloudVerticalTabItem));
            UnSelectedEvent = EventManager.RegisterRoutedEvent("UnSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CloudVerticalTabItem));
        }
    }

}
