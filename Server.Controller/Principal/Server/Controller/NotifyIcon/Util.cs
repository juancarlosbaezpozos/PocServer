using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Principal.Server.Controller.NotifyIcon.Interop;

namespace Principal.Server.Controller.NotifyIcon;

internal static class Util
{
    public static readonly object SyncRoot;

    private static readonly bool isDesignMode;

    public static bool IsDesignMode => isDesignMode;

    static Util()
    {
        SyncRoot = new object();
        isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
    }

    //public static Window CreateHelperWindow()
    //{
    //    return new Window
    //    {
    //        Width = 0.0,
    //        Height = 0.0,
    //        ShowInTaskbar = false,
    //        WindowStyle = WindowStyle.None,
    //        AllowsTransparency = true,
    //        Opacity = 0.0
    //    };
    //}

    public static bool WriteIconData(ref NotifyIconData data, NotifyCommand command)
    {
        return WriteIconData(ref data, command, data.ValidMembers);
    }

    public static bool WriteIconData(ref NotifyIconData data, NotifyCommand command, IconDataMembers flags)
    {
        if (IsDesignMode)
        {
            return true;
        }
        data.ValidMembers = flags;
        lock (SyncRoot)
        {
            return WinApi.Shell_NotifyIcon(command, ref data);
        }
    }

    public static BalloonFlags GetBalloonFlag(this BalloonIcon icon)
    {
        return icon switch
        {
            BalloonIcon.None => BalloonFlags.None,
            BalloonIcon.Info => BalloonFlags.Info,
            BalloonIcon.Warning => BalloonFlags.Warning,
            BalloonIcon.Error => BalloonFlags.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(icon)),
        };
    }

    public static Icon ToIcon(this ImageSource imageSource)
    {
        return imageSource == null ? null : new Icon((Application.GetResourceStream(new Uri(imageSource.ToString())) ?? throw new ArgumentException($"La imágen '{imageSource}' no pudo ser resuelto.")).Stream);
    }

    public static bool Is<T>(this T value, params T[] candidates)
    {
        if (candidates == null)
        {
            return false;
        }

        foreach (T val in candidates)
        {
            if (value.Equals(val))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsMatch(this MouseEvent me, PopupActivationMode activationMode)
    {
        return activationMode switch
        {
            PopupActivationMode.LeftClick => me == MouseEvent.IconLeftMouseUp,
            PopupActivationMode.RightClick => me == MouseEvent.IconRightMouseUp,
            PopupActivationMode.LeftOrRightClick => me.Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconRightMouseUp),
            PopupActivationMode.LeftOrDoubleClick => me.Is(MouseEvent.IconLeftMouseUp, MouseEvent.IconDoubleClick),
            PopupActivationMode.DoubleClick => me.Is(MouseEvent.IconDoubleClick),
            PopupActivationMode.MiddleClick => me == MouseEvent.IconMiddleMouseUp,
            PopupActivationMode.All => me != MouseEvent.MouseMove,
            _ => throw new ArgumentOutOfRangeException(nameof(activationMode)),
        };
    }

    public static void ExecuteIfEnabled(this ICommand command, object commandParameter, IInputElement target)
    {
        if (command == null)
        {
            return;
        }

        if (command is RoutedCommand routedCommand)
        {
            if (routedCommand.CanExecute(commandParameter, target))
            {
                routedCommand.Execute(commandParameter, target);
            }
        }
        else if (command.CanExecute(commandParameter))
        {
            command.Execute(commandParameter);
        }
    }

    internal static Dispatcher GetDispatcher(this DispatcherObject source)
    {
        if (Application.Current != null)
        {
            return Application.Current.Dispatcher;
        }

        if (source.Dispatcher != null)
        {
            return source.Dispatcher;
        }

        return Dispatcher.CurrentDispatcher;
    }

    public static bool IsDataContextDataBound(this FrameworkElement element)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return element.GetBindingExpression(FrameworkElement.DataContextProperty) != null;
    }
}
