using System;
using System.Windows;

namespace Principal.Server.Controller.NotifyIcon;

internal static class RoutedEventHelper
{
    internal static void RaiseEvent(DependencyObject target, RoutedEventArgs args)
    {
        switch (target)
        {
            case UIElement element:
                element.RaiseEvent(args);
                break;
            case ContentElement contentElement:
                contentElement.RaiseEvent(args);
                break;
        }
    }

    internal static void AddHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
    {
        switch (element)
        {
            case UIElement uIElement:
                uIElement.AddHandler(routedEvent, handler);
                break;
            case ContentElement contentElement:
                contentElement.AddHandler(routedEvent, handler);
                break;
        }
    }

    internal static void RemoveHandler(DependencyObject element, RoutedEvent routedEvent, Delegate handler)
    {
        switch (element)
        {
            case UIElement uIElement:
                uIElement.RemoveHandler(routedEvent, handler);
                break;
            case ContentElement contentElement:
                contentElement.RemoveHandler(routedEvent, handler);
                break;
        }
    }
}
