using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Principal.Server.Controller.NotifyIcon.Interop;
using Point = Principal.Server.Controller.NotifyIcon.Interop.Point;

namespace Principal.Server.Controller.NotifyIcon;

public class TaskbarIcon : FrameworkElement, IDisposable
{
    private NotifyIconData iconData;

    private readonly WindowMessageSink messageSink;

    private Action singleClickTimerAction;

    private readonly Timer singleClickTimer;

    private readonly Timer balloonCloseTimer;

    private double scalingFactor = double.NaN;

    public const string CategoryName = "NotifyIcon";

    private static readonly DependencyPropertyKey TrayPopupResolvedPropertyKey;

    public static readonly DependencyProperty TrayPopupResolvedProperty;

    private static readonly DependencyPropertyKey TrayToolTipResolvedPropertyKey;

    public static readonly DependencyProperty TrayToolTipResolvedProperty;

    private static readonly DependencyPropertyKey CustomBalloonPropertyKey;

    public static readonly DependencyProperty CustomBalloonProperty;

    private Icon icon;

    public static readonly DependencyProperty IconSourceProperty;

    public static readonly DependencyProperty ToolTipTextProperty;

    public static readonly DependencyProperty TrayToolTipProperty;

    public static readonly DependencyProperty TrayPopupProperty;

    public static readonly DependencyProperty MenuActivationProperty;

    public static readonly DependencyProperty PopupActivationProperty;

    public static readonly DependencyProperty DoubleClickCommandProperty;

    public static readonly DependencyProperty DoubleClickCommandParameterProperty;

    public static readonly DependencyProperty DoubleClickCommandTargetProperty;

    public static readonly DependencyProperty LeftClickCommandProperty;

    public static readonly DependencyProperty LeftClickCommandParameterProperty;

    public static readonly DependencyProperty LeftClickCommandTargetProperty;

    public static readonly RoutedEvent TrayLeftMouseDownEvent;

    public static readonly RoutedEvent TrayRightMouseDownEvent;

    public static readonly RoutedEvent TrayMiddleMouseDownEvent;

    public static readonly RoutedEvent TrayLeftMouseUpEvent;

    public static readonly RoutedEvent TrayRightMouseUpEvent;

    public static readonly RoutedEvent TrayMiddleMouseUpEvent;

    public static readonly RoutedEvent TrayMouseDoubleClickEvent;

    public static readonly RoutedEvent TrayMouseMoveEvent;

    public static readonly RoutedEvent TrayBalloonTipShownEvent;

    public static readonly RoutedEvent TrayBalloonTipClosedEvent;

    public static readonly RoutedEvent TrayBalloonTipClickedEvent;

    public static readonly RoutedEvent TrayContextMenuOpenEvent;

    public static readonly RoutedEvent PreviewTrayContextMenuOpenEvent;

    public static readonly RoutedEvent TrayPopupOpenEvent;

    public static readonly RoutedEvent PreviewTrayPopupOpenEvent;

    public static readonly RoutedEvent TrayToolTipOpenEvent;

    public static readonly RoutedEvent PreviewTrayToolTipOpenEvent;

    public static readonly RoutedEvent TrayToolTipCloseEvent;

    public static readonly RoutedEvent PreviewTrayToolTipCloseEvent;

    public static readonly RoutedEvent PopupOpenedEvent;

    public static readonly RoutedEvent ToolTipOpenedEvent;

    public static readonly RoutedEvent ToolTipCloseEvent;

    public static readonly RoutedEvent BalloonShowingEvent;

    public static readonly RoutedEvent BalloonClosingEvent;

    public static readonly DependencyProperty ParentTaskbarIconProperty;

    public bool IsTaskbarIconCreated { get; private set; }

    public bool SupportsCustomToolTips => messageSink.Version == NotifyIconVersion.Vista;

    private bool IsPopupOpen
    {
        get
        {
            var trayPopupResolved = TrayPopupResolved;
            var contextMenu = base.ContextMenu;
            var customBalloon = CustomBalloon;
            if ((trayPopupResolved == null || !trayPopupResolved.IsOpen) && (contextMenu == null || !contextMenu.IsOpen))
            {
                return customBalloon?.IsOpen ?? false;
            }
            return true;
        }
    }

    public bool IsDisposed { get; private set; }

    [Category(CategoryName)]
    public Popup TrayPopupResolved => (Popup)GetValue(TrayPopupResolvedProperty);

    [Category(CategoryName)]
    [Browsable(true)]
    [Bindable(true)]
    public ToolTip TrayToolTipResolved => (ToolTip)GetValue(TrayToolTipResolvedProperty);

    public Popup CustomBalloon => (Popup)GetValue(CustomBalloonProperty);

    [Browsable(false)]
    public Icon Icon
    {
        get => icon;
        set
        {
            icon = value;
            iconData.IconHandle = ((value == null) ? IntPtr.Zero : icon.Handle);
            Util.WriteIconData(ref iconData, NotifyCommand.Modify, IconDataMembers.Icon);
        }
    }

    [Category(CategoryName)]
    [Description("Sets the displayed taskbar icon.")]
    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    [Category(CategoryName)]
    [Description("Alternative to a fully blown ToolTip, which is only displayed on Vista and above.")]
    public string ToolTipText
    {
        get => (string)GetValue(ToolTipTextProperty);
        set => SetValue(ToolTipTextProperty, value);
    }

    [Category(CategoryName)]
    [Description("Custom UI element that is displayed as a tooltip. Only on Vista and above")]
    public UIElement TrayToolTip
    {
        get => (UIElement)GetValue(TrayToolTipProperty);
        set => SetValue(TrayToolTipProperty, value);
    }

    [Category(CategoryName)]
    [Description("Displayed as a Popup if the user clicks on the taskbar icon.")]
    public UIElement TrayPopup
    {
        get => (UIElement)GetValue(TrayPopupProperty);
        set => SetValue(TrayPopupProperty, value);
    }

    [Category(CategoryName)]
    [Description("Defines what mouse events display the context menu.")]
    public PopupActivationMode MenuActivation
    {
        get => (PopupActivationMode)GetValue(MenuActivationProperty);
        set => SetValue(MenuActivationProperty, value);
    }

    [Category(CategoryName)]
    [Description("Defines what mouse events display the TaskbarIconPopup.")]
    public PopupActivationMode PopupActivation
    {
        get => (PopupActivationMode)GetValue(PopupActivationProperty);
        set => SetValue(PopupActivationProperty, value);
    }

    [Category(CategoryName)]
    [Description("A command that is being executed if the tray icon is being double-clicked.")]
    public ICommand DoubleClickCommand
    {
        get => (ICommand)GetValue(DoubleClickCommandProperty);
        set => SetValue(DoubleClickCommandProperty, value);
    }

    [Category(CategoryName)]
    [Description("Parameter to submit to the DoubleClickCommand when the user double clicks on the NotifyIcon.")]
    public object DoubleClickCommandParameter
    {
        get => GetValue(DoubleClickCommandParameterProperty);
        set => SetValue(DoubleClickCommandParameterProperty, value);
    }

    [Category(CategoryName)]
    [Description("The target of the command that is fired if the notify icon is double clicked.")]
    public IInputElement DoubleClickCommandTarget
    {
        get => (IInputElement)GetValue(DoubleClickCommandTargetProperty);
        set => SetValue(DoubleClickCommandTargetProperty, value);
    }

    [Category(CategoryName)]
    [Description("A command that is being executed if the tray icon is being left-clicked.")]
    public ICommand LeftClickCommand
    {
        get => (ICommand)GetValue(LeftClickCommandProperty);
        set => SetValue(LeftClickCommandProperty, value);
    }

    [Category(CategoryName)]
    [Description("The target of the command that is fired if the notify icon is clicked with the left mouse button.")]
    public object LeftClickCommandParameter
    {
        get => GetValue(LeftClickCommandParameterProperty);
        set => SetValue(LeftClickCommandParameterProperty, value);
    }

    [Category(CategoryName)]
    [Description("The target of the command that is fired if the notify icon is clicked with the left mouse button.")]
    public IInputElement LeftClickCommandTarget
    {
        get => (IInputElement)GetValue(LeftClickCommandTargetProperty);
        set => SetValue(LeftClickCommandTargetProperty, value);
    }

    [Category(CategoryName)]
    public event RoutedEventHandler TrayLeftMouseDown
    {
        add => AddHandler(TrayLeftMouseDownEvent, value);
        remove => RemoveHandler(TrayLeftMouseDownEvent, value);
    }

    public event RoutedEventHandler TrayRightMouseDown
    {
        add => AddHandler(TrayRightMouseDownEvent, value);
        remove => RemoveHandler(TrayRightMouseDownEvent, value);
    }

    public event RoutedEventHandler TrayMiddleMouseDown
    {
        add => AddHandler(TrayMiddleMouseDownEvent, value);
        remove => RemoveHandler(TrayMiddleMouseDownEvent, value);
    }

    public event RoutedEventHandler TrayLeftMouseUp
    {
        add => AddHandler(TrayLeftMouseUpEvent, value);
        remove => RemoveHandler(TrayLeftMouseUpEvent, value);
    }

    public event RoutedEventHandler TrayRightMouseUp
    {
        add => AddHandler(TrayRightMouseUpEvent, value);
        remove => RemoveHandler(TrayRightMouseUpEvent, value);
    }

    public event RoutedEventHandler TrayMiddleMouseUp
    {
        add => AddHandler(TrayMiddleMouseUpEvent, value);
        remove => RemoveHandler(TrayMiddleMouseUpEvent, value);
    }

    public event RoutedEventHandler TrayMouseDoubleClick
    {
        add => AddHandler(TrayMouseDoubleClickEvent, value);
        remove => RemoveHandler(TrayMouseDoubleClickEvent, value);
    }

    public event RoutedEventHandler TrayMouseMove
    {
        add => AddHandler(TrayMouseMoveEvent, value);
        remove => RemoveHandler(TrayMouseMoveEvent, value);
    }

    public event RoutedEventHandler TrayBalloonTipShown
    {
        add => AddHandler(TrayBalloonTipShownEvent, value);
        remove => RemoveHandler(TrayBalloonTipShownEvent, value);
    }

    public event RoutedEventHandler TrayBalloonTipClosed
    {
        add => AddHandler(TrayBalloonTipClosedEvent, value);
        remove => RemoveHandler(TrayBalloonTipClosedEvent, value);
    }

    public event RoutedEventHandler TrayBalloonTipClicked
    {
        add => AddHandler(TrayBalloonTipClickedEvent, value);
        remove => RemoveHandler(TrayBalloonTipClickedEvent, value);
    }

    public event RoutedEventHandler TrayContextMenuOpen
    {
        add => AddHandler(TrayContextMenuOpenEvent, value);
        remove => RemoveHandler(TrayContextMenuOpenEvent, value);
    }

    public event RoutedEventHandler PreviewTrayContextMenuOpen
    {
        add => AddHandler(PreviewTrayContextMenuOpenEvent, value);
        remove => RemoveHandler(PreviewTrayContextMenuOpenEvent, value);
    }

    public event RoutedEventHandler TrayPopupOpen
    {
        add => AddHandler(TrayPopupOpenEvent, value);
        remove => RemoveHandler(TrayPopupOpenEvent, value);
    }

    public event RoutedEventHandler PreviewTrayPopupOpen
    {
        add => AddHandler(PreviewTrayPopupOpenEvent, value);
        remove => RemoveHandler(PreviewTrayPopupOpenEvent, value);
    }

    public event RoutedEventHandler TrayToolTipOpen
    {
        add => AddHandler(TrayToolTipOpenEvent, value);
        remove => RemoveHandler(TrayToolTipOpenEvent, value);
    }

    public event RoutedEventHandler PreviewTrayToolTipOpen
    {
        add => AddHandler(PreviewTrayToolTipOpenEvent, value);
        remove => RemoveHandler(PreviewTrayToolTipOpenEvent, value);
    }

    public event RoutedEventHandler TrayToolTipClose
    {
        add => AddHandler(TrayToolTipCloseEvent, value);
        remove => RemoveHandler(TrayToolTipCloseEvent, value);
    }

    public event RoutedEventHandler PreviewTrayToolTipClose
    {
        add => AddHandler(PreviewTrayToolTipCloseEvent, value);
        remove => RemoveHandler(PreviewTrayToolTipCloseEvent, value);
    }

    public TaskbarIcon()
    {
        messageSink = (Util.IsDesignMode ? WindowMessageSink.CreateEmpty() : new WindowMessageSink(NotifyIconVersion.Win95));
        iconData = NotifyIconData.CreateDefault(messageSink.MessageWindowHandle);
        CreateTaskbarIcon();
        messageSink.MouseEventReceived += OnMouseEvent;
        messageSink.TaskbarCreated += OnTaskbarCreated;
        messageSink.ChangeToolTipStateRequest += OnToolTipChange;
        messageSink.BalloonToolTipChanged += OnBalloonToolTipChanged;
        singleClickTimer = new Timer(DoSingleClickAction);
        balloonCloseTimer = new Timer(CloseBalloonCallback);
        if (Application.Current != null)
        {
            Application.Current.Exit += OnExit;
        }
    }

    public void ShowCustomBalloon(UIElement balloon, PopupAnimation animation, int? timeout)
    {
        var dispatcher = this.GetDispatcher();
        if (!dispatcher.CheckAccess())
        {
            Action method = delegate
            {
                ShowCustomBalloon(balloon, animation, timeout);
            };
            dispatcher.Invoke(DispatcherPriority.Normal, method);
            return;
        }
        if (balloon == null)
        {
            throw new ArgumentNullException(nameof(balloon));
        }
        if (timeout.HasValue && timeout < 500)
        {
            var format = "Invalid timeout of {0} milliseconds. Timeout must be at least 500 ms";
            format = string.Format(format, timeout);
            throw new ArgumentOutOfRangeException(nameof(timeout), format);
        }
        EnsureNotDisposed();
        lock (this)
        {
            CloseBalloon();
        }
        var popup = new Popup();
        popup.AllowsTransparency = true;
        UpdateDataContext(popup, null, base.DataContext);
        popup.PopupAnimation = animation;
        var popup2 = LogicalTreeHelper.GetParent(balloon) as Popup;
        if (popup2 != null)
        {
            popup2.Child = null;
        }
        if (popup2 != null)
        {
            throw new InvalidOperationException($"Cannot display control [{balloon}] in a new balloon popup - that control already has a parent. You may consider creating new balloons every time you want to show one.");
        }
        popup.Child = balloon;
        popup.Placement = PlacementMode.AbsolutePoint;
        popup.StaysOpen = true;
        var trayLocation = TrayInfo.GetTrayLocation();
        trayLocation = GetDeviceCoordinates(trayLocation);
        popup.HorizontalOffset = trayLocation.X - 1;
        popup.VerticalOffset = trayLocation.Y - 1;
        lock (this)
        {
            SetCustomBalloon(popup);
        }
        SetParentTaskbarIcon(balloon, this);
        RaiseBalloonShowingEvent(balloon, this);
        popup.IsOpen = true;
        if (timeout.HasValue)
        {
            balloonCloseTimer.Change(timeout.Value, -1);
        }
    }

    public void ResetBalloonCloseTimer()
    {
        if (IsDisposed)
        {
            return;
        }
        lock (this)
        {
            balloonCloseTimer.Change(-1, -1);
        }
    }

    public void CloseBalloon()
    {
        if (IsDisposed)
        {
            return;
        }
        var dispatcher = this.GetDispatcher();
        if (!dispatcher.CheckAccess())
        {
            var method = CloseBalloon;
            dispatcher.Invoke(DispatcherPriority.Normal, method);
            return;
        }
        lock (this)
        {
            balloonCloseTimer.Change(-1, -1);
            var customBalloon = CustomBalloon;
            if (customBalloon == null)
            {
                return;
            }
            var child = customBalloon.Child;
            if (!RaiseBalloonClosingEvent(child, this).Handled)
            {
                customBalloon.IsOpen = false;
                customBalloon.Child = null;
                if (child != null)
                {
                    SetParentTaskbarIcon(child, null);
                }
            }
            SetCustomBalloon(null);
        }
    }

    private void CloseBalloonCallback(object state)
    {
        if (!IsDisposed)
        {
            var callback = CloseBalloon;
            this.GetDispatcher().Invoke(callback);
        }
    }

    private void OnMouseEvent(MouseEvent me)
    {
        if (IsDisposed)
        {
            return;
        }
        switch (me)
        {
            case MouseEvent.MouseMove:
                RaiseTrayMouseMoveEvent();
                return;
            case MouseEvent.IconRightMouseDown:
                RaiseTrayRightMouseDownEvent();
                break;
            case MouseEvent.IconLeftMouseDown:
                RaiseTrayLeftMouseDownEvent();
                break;
            case MouseEvent.IconRightMouseUp:
                RaiseTrayRightMouseUpEvent();
                break;
            case MouseEvent.IconLeftMouseUp:
                RaiseTrayLeftMouseUpEvent();
                break;
            case MouseEvent.IconMiddleMouseDown:
                RaiseTrayMiddleMouseDownEvent();
                break;
            case MouseEvent.IconMiddleMouseUp:
                RaiseTrayMiddleMouseUpEvent();
                break;
            case MouseEvent.IconDoubleClick:
                singleClickTimer.Change(-1, -1);
                RaiseTrayMouseDoubleClickEvent();
                break;
            case MouseEvent.BalloonToolTipClicked:
                RaiseTrayBalloonTipClickedEvent();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(me), "Missing handler for mouse event flag: " + me);
        }
        var cursorPosition = default(Point);
        if (messageSink.Version == NotifyIconVersion.Vista)
        {
            WinApi.GetPhysicalCursorPos(ref cursorPosition);
        }
        else
        {
            WinApi.GetCursorPos(ref cursorPosition);
        }
        cursorPosition = GetDeviceCoordinates(cursorPosition);
        var flag = false;
        if (me.IsMatch(PopupActivation))
        {
            if (me == MouseEvent.IconLeftMouseUp)
            {
                singleClickTimerAction = delegate
                {
                    LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                    ShowTrayPopup(cursorPosition);
                };
                singleClickTimer.Change(WinApi.GetDoubleClickTime(), -1);
                flag = true;
            }
            else
            {
                ShowTrayPopup(cursorPosition);
            }
        }
        if (me.IsMatch(MenuActivation))
        {
            if (me == MouseEvent.IconLeftMouseUp)
            {
                singleClickTimerAction = delegate
                {
                    LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
                    ShowContextMenu(cursorPosition);
                };
                singleClickTimer.Change(WinApi.GetDoubleClickTime(), -1);
                flag = true;
            }
            else
            {
                ShowContextMenu(cursorPosition);
            }
        }
        if (me == MouseEvent.IconLeftMouseUp && !flag)
        {
            singleClickTimerAction = delegate
            {
                LeftClickCommand.ExecuteIfEnabled(LeftClickCommandParameter, LeftClickCommandTarget ?? this);
            };
            singleClickTimer.Change(WinApi.GetDoubleClickTime(), -1);
        }
    }

    private void OnToolTipChange(bool visible)
    {
        if (TrayToolTipResolved == null)
        {
            return;
        }
        if (visible)
        {
            if (!IsPopupOpen && !RaisePreviewTrayToolTipOpenEvent().Handled)
            {
                TrayToolTipResolved.IsOpen = true;
                if (TrayToolTip != null)
                {
                    RaiseToolTipOpenedEvent(TrayToolTip);
                }
                RaiseTrayToolTipOpenEvent();
            }
        }
        else if (!RaisePreviewTrayToolTipCloseEvent().Handled)
        {
            if (TrayToolTip != null)
            {
                RaiseToolTipCloseEvent(TrayToolTip);
            }
            TrayToolTipResolved.IsOpen = false;
            RaiseTrayToolTipCloseEvent();
        }
    }

    private void CreateCustomToolTip()
    {
        var toolTip = TrayToolTip as ToolTip;
        if (toolTip == null && TrayToolTip != null)
        {
            toolTip = new ToolTip();
            toolTip.Placement = PlacementMode.Mouse;
            toolTip.HasDropShadow = false;
            toolTip.BorderThickness = new Thickness(0.0);
            toolTip.Background = System.Windows.Media.Brushes.Transparent;
            toolTip.StaysOpen = true;
            toolTip.Content = TrayToolTip;
        }
        else if (toolTip == null && !string.IsNullOrEmpty(ToolTipText))
        {
            toolTip = new ToolTip();
            toolTip.Content = ToolTipText;
        }
        if (toolTip != null)
        {
            UpdateDataContext(toolTip, null, base.DataContext);
        }
        SetTrayToolTipResolved(toolTip);
    }

    private void WriteToolTipSettings()
    {
        iconData.ToolTipText = ToolTipText;
        if (messageSink.Version == NotifyIconVersion.Vista && string.IsNullOrEmpty(iconData.ToolTipText) && TrayToolTipResolved != null)
        {
            iconData.ToolTipText = "ToolTip";
        }
        Util.WriteIconData(ref iconData, NotifyCommand.Modify, IconDataMembers.Tip);
    }

    private void CreatePopup()
    {
        var popup = TrayPopup as Popup;
        if (popup == null && TrayPopup != null)
        {
            popup = new Popup();
            popup.AllowsTransparency = true;
            popup.PopupAnimation = PopupAnimation.None;
            popup.Child = TrayPopup;
            popup.Placement = PlacementMode.AbsolutePoint;
            popup.StaysOpen = false;
        }
        if (popup != null)
        {
            UpdateDataContext(popup, null, base.DataContext);
        }
        SetTrayPopupResolved(popup);
    }

    private void ShowTrayPopup(Point cursorPosition)
    {
        if (IsDisposed || RaisePreviewTrayPopupOpenEvent().Handled || TrayPopup == null)
        {
            return;
        }
        TrayPopupResolved.Placement = PlacementMode.AbsolutePoint;
        TrayPopupResolved.HorizontalOffset = cursorPosition.X;
        TrayPopupResolved.VerticalOffset = cursorPosition.Y;
        TrayPopupResolved.IsOpen = true;
        var intPtr = IntPtr.Zero;
        if (TrayPopupResolved.Child != null)
        {
            var hwndSource = (HwndSource)PresentationSource.FromVisual(TrayPopupResolved.Child);
            if (hwndSource != null)
            {
                intPtr = hwndSource.Handle;
            }
        }
        if (intPtr == IntPtr.Zero)
        {
            intPtr = messageSink.MessageWindowHandle;
        }
        WinApi.SetForegroundWindow(intPtr);
        if (TrayPopup != null)
        {
            RaisePopupOpenedEvent(TrayPopup);
        }
        RaiseTrayPopupOpenEvent();
    }

    private void ShowContextMenu(Point cursorPosition)
    {
        if (!IsDisposed && !RaisePreviewTrayContextMenuOpenEvent().Handled && base.ContextMenu != null)
        {
            base.ContextMenu.IsOpen = true;
            var intPtr = IntPtr.Zero;
            var hwndSource = (HwndSource)PresentationSource.FromVisual(base.ContextMenu);
            if (hwndSource != null)
            {
                intPtr = hwndSource.Handle;
            }
            if (intPtr == IntPtr.Zero)
            {
                intPtr = messageSink.MessageWindowHandle;
            }
            WinApi.SetForegroundWindow(intPtr);
            RaiseTrayContextMenuOpenEvent();
        }
    }

    private void OnBalloonToolTipChanged(bool visible)
    {
        if (visible)
        {
            RaiseTrayBalloonTipShownEvent();
        }
        else
        {
            RaiseTrayBalloonTipClosedEvent();
        }
    }

    public void ShowBalloonTip(string title, string message, BalloonIcon symbol)
    {
        lock (this)
        {
            ShowBalloonTip(title, message, symbol.GetBalloonFlag(), IntPtr.Zero);
        }
    }

    public void ShowBalloonTip(string title, string message, Icon customIcon)
    {
        if (customIcon == null)
        {
            throw new ArgumentNullException(nameof(customIcon));
        }
        lock (this)
        {
            ShowBalloonTip(title, message, BalloonFlags.User, customIcon.Handle);
        }
    }

    private void ShowBalloonTip(string title, string message, BalloonFlags flags, IntPtr balloonIconHandle)
    {
        EnsureNotDisposed();
        iconData.BalloonText = message ?? string.Empty;
        iconData.BalloonTitle = title ?? string.Empty;
        iconData.BalloonFlags = flags;
        iconData.CustomBalloonIconHandle = balloonIconHandle;
        Util.WriteIconData(ref iconData, NotifyCommand.Modify, IconDataMembers.Icon | IconDataMembers.Info);
    }

    public void HideBalloonTip()
    {
        EnsureNotDisposed();
        iconData.BalloonText = (iconData.BalloonTitle = string.Empty);
        Util.WriteIconData(ref iconData, NotifyCommand.Modify, IconDataMembers.Info);
    }

    private void DoSingleClickAction(object state)
    {
        if (!IsDisposed)
        {
            var action = singleClickTimerAction;
            if (action != null)
            {
                singleClickTimerAction = null;
                this.GetDispatcher().Invoke(action);
            }
        }
    }

    private void SetVersion()
    {
        iconData.VersionOrTimeout = 4u;
        var flag = WinApi.Shell_NotifyIcon(NotifyCommand.SetVersion, ref iconData);
        if (!flag)
        {
            iconData.VersionOrTimeout = 3u;
            flag = Util.WriteIconData(ref iconData, NotifyCommand.SetVersion);
        }

        if (!flag)
        {
            iconData.VersionOrTimeout = 0u;
            flag = Util.WriteIconData(ref iconData, NotifyCommand.SetVersion);
        }
    }

    private void OnTaskbarCreated()
    {
        IsTaskbarIconCreated = false;
        CreateTaskbarIcon();
    }

    private void CreateTaskbarIcon()
    {
        lock (this)
        {
            if (!IsTaskbarIconCreated && Util.WriteIconData(ref iconData, NotifyCommand.Add, IconDataMembers.Message | IconDataMembers.Icon | IconDataMembers.Tip))
            {
                SetVersion();
                messageSink.Version = (NotifyIconVersion)iconData.VersionOrTimeout;
                IsTaskbarIconCreated = true;
            }
        }
    }

    private void RemoveTaskbarIcon()
    {
        lock (this)
        {
            if (IsTaskbarIconCreated)
            {
                Util.WriteIconData(ref iconData, NotifyCommand.Delete, IconDataMembers.Message);
                IsTaskbarIconCreated = false;
            }
        }
    }

    private Point GetDeviceCoordinates(Point point)
    {
        if (double.IsNaN(scalingFactor))
        {
            var presentationSource = PresentationSource.FromVisual(this);
            if (presentationSource == null)
            {
                scalingFactor = 1.0;
            }
            else
            {
                scalingFactor = 1.0 / presentationSource.CompositionTarget.TransformToDevice.M11;
            }
        }

        if (scalingFactor == 1.0)
        {
            return point;
        }

        var result = default(Point);
        result.X = (int)((double)point.X * scalingFactor);
        result.Y = (int)((double)point.Y * scalingFactor);
        return result;
    }

    private void EnsureNotDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(base.Name ?? GetType().FullName);
        }
    }

    private void OnExit(object sender, EventArgs e)
    {
        Dispose();
    }

    ~TaskbarIcon()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (IsDisposed || !disposing)
        {
            return;
        }
        lock (this)
        {
            IsDisposed = true;
            if (Application.Current != null)
            {
                Application.Current.Exit -= OnExit;
            }
            singleClickTimer.Dispose();
            balloonCloseTimer.Dispose();
            messageSink.Dispose();
            RemoveTaskbarIcon();
        }
    }

    protected void SetTrayPopupResolved(Popup value)
    {
        SetValue(TrayPopupResolvedPropertyKey, value);
    }

    protected void SetTrayToolTipResolved(ToolTip value)
    {
        SetValue(TrayToolTipResolvedPropertyKey, value);
    }

    protected void SetCustomBalloon(Popup value)
    {
        SetValue(CustomBalloonPropertyKey, value);
    }

    private static void IconSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnIconSourcePropertyChanged(e);
    }

    private void OnIconSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        var imageSource = (ImageSource)e.NewValue;
        if (!Util.IsDesignMode)
        {
            Icon = imageSource.ToIcon();
        }
    }

    private static void ToolTipTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnToolTipTextPropertyChanged(e);
    }

    private void OnToolTipTextPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (TrayToolTip == null)
        {
            var trayToolTipResolved = TrayToolTipResolved;
            if (trayToolTipResolved == null)
            {
                CreateCustomToolTip();
            }
            else
            {
                trayToolTipResolved.Content = e.NewValue;
            }
        }

        WriteToolTipSettings();
    }

    private static void TrayToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnTrayToolTipPropertyChanged(e);
    }

    private void OnTrayToolTipPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        CreateCustomToolTip();
        if (e.OldValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.OldValue, null);
        }
        if (e.NewValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.NewValue, this);
        }
        WriteToolTipSettings();
    }

    private static void TrayPopupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnTrayPopupPropertyChanged(e);
    }

    private void OnTrayPopupPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.OldValue, null);
        }
        if (e.NewValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.NewValue, this);
        }

        CreatePopup();
    }

    private static void VisibilityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnVisibilityPropertyChanged(e);
    }

    private void OnVisibilityPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if ((Visibility)e.NewValue == Visibility.Visible)
        {
            CreateTaskbarIcon();
        }
        else
        {
            RemoveTaskbarIcon();
        }
    }

    private void UpdateDataContext(FrameworkElement target, object oldDataContextValue, object newDataContextValue)
    {
        if (target != null && !target.IsDataContextDataBound() && (this == target.DataContext || object.Equals(oldDataContextValue, target.DataContext)))
        {
            target.DataContext = newDataContextValue ?? this;
        }
    }

    private static void DataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnDataContextPropertyChanged(e);
    }

    private void OnDataContextPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        var newValue = e.NewValue;
        var oldValue = e.OldValue;
        UpdateDataContext(TrayPopupResolved, oldValue, newValue);
        UpdateDataContext(TrayToolTipResolved, oldValue, newValue);
        UpdateDataContext(base.ContextMenu, oldValue, newValue);
    }

    private static void ContextMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((TaskbarIcon)d).OnContextMenuPropertyChanged(e);
    }

    private void OnContextMenuPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.OldValue, null);
        }

        if (e.NewValue != null)
        {
            SetParentTaskbarIcon((DependencyObject)e.NewValue, this);
        }

        UpdateDataContext((ContextMenu)e.NewValue, null, base.DataContext);
    }

    protected RoutedEventArgs RaiseTrayLeftMouseDownEvent()
    {
        return RaiseTrayLeftMouseDownEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayLeftMouseDownEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayLeftMouseDownEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayRightMouseDownEvent()
    {
        return RaiseTrayRightMouseDownEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayRightMouseDownEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayRightMouseDownEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayMiddleMouseDownEvent()
    {
        return RaiseTrayMiddleMouseDownEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayMiddleMouseDownEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayMiddleMouseDownEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayLeftMouseUpEvent()
    {
        return RaiseTrayLeftMouseUpEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayLeftMouseUpEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayLeftMouseUpEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayRightMouseUpEvent()
    {
        return RaiseTrayRightMouseUpEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayRightMouseUpEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayRightMouseUpEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayMiddleMouseUpEvent()
    {
        return RaiseTrayMiddleMouseUpEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayMiddleMouseUpEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayMiddleMouseUpEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayMouseDoubleClickEvent()
    {
        var result = RaiseTrayMouseDoubleClickEvent(this);
        DoubleClickCommand.ExecuteIfEnabled(DoubleClickCommandParameter, DoubleClickCommandTarget ?? this);
        return result;
    }

    internal static RoutedEventArgs RaiseTrayMouseDoubleClickEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayMouseDoubleClickEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayMouseMoveEvent()
    {
        return RaiseTrayMouseMoveEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayMouseMoveEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayMouseMoveEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayBalloonTipShownEvent()
    {
        return RaiseTrayBalloonTipShownEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayBalloonTipShownEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayBalloonTipShownEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayBalloonTipClosedEvent()
    {
        return RaiseTrayBalloonTipClosedEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayBalloonTipClosedEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayBalloonTipClosedEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayBalloonTipClickedEvent()
    {
        return RaiseTrayBalloonTipClickedEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayBalloonTipClickedEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayBalloonTipClickedEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayContextMenuOpenEvent()
    {
        return RaiseTrayContextMenuOpenEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayContextMenuOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayContextMenuOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaisePreviewTrayContextMenuOpenEvent()
    {
        return RaisePreviewTrayContextMenuOpenEvent(this);
    }

    internal static RoutedEventArgs RaisePreviewTrayContextMenuOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.PreviewTrayContextMenuOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayPopupOpenEvent()
    {
        return RaiseTrayPopupOpenEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayPopupOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayPopupOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaisePreviewTrayPopupOpenEvent()
    {
        return RaisePreviewTrayPopupOpenEvent(this);
    }

    internal static RoutedEventArgs RaisePreviewTrayPopupOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.PreviewTrayPopupOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayToolTipOpenEvent()
    {
        return RaiseTrayToolTipOpenEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayToolTipOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayToolTipOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaisePreviewTrayToolTipOpenEvent()
    {
        return RaisePreviewTrayToolTipOpenEvent(this);
    }

    internal static RoutedEventArgs RaisePreviewTrayToolTipOpenEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.PreviewTrayToolTipOpenEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaiseTrayToolTipCloseEvent()
    {
        return RaiseTrayToolTipCloseEvent(this);
    }

    internal static RoutedEventArgs RaiseTrayToolTipCloseEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.TrayToolTipCloseEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    protected RoutedEventArgs RaisePreviewTrayToolTipCloseEvent()
    {
        return RaisePreviewTrayToolTipCloseEvent(this);
    }

    internal static RoutedEventArgs RaisePreviewTrayToolTipCloseEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = TaskbarIcon.PreviewTrayToolTipCloseEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static void AddPopupOpenedHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.AddHandler(element, PopupOpenedEvent, handler);
    }

    public static void RemovePopupOpenedHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.RemoveHandler(element, PopupOpenedEvent, handler);
    }

    internal static RoutedEventArgs RaisePopupOpenedEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = PopupOpenedEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static void AddToolTipOpenedHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.AddHandler(element, ToolTipOpenedEvent, handler);
    }

    public static void RemoveToolTipOpenedHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.RemoveHandler(element, ToolTipOpenedEvent, handler);
    }

    internal static RoutedEventArgs RaiseToolTipOpenedEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = ToolTipOpenedEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static void AddToolTipCloseHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.AddHandler(element, ToolTipCloseEvent, handler);
    }

    public static void RemoveToolTipCloseHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.RemoveHandler(element, ToolTipCloseEvent, handler);
    }

    internal static RoutedEventArgs RaiseToolTipCloseEvent(DependencyObject target)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs();
        routedEventArgs.RoutedEvent = ToolTipCloseEvent;
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static void AddBalloonShowingHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.AddHandler(element, BalloonShowingEvent, handler);
    }

    public static void RemoveBalloonShowingHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.RemoveHandler(element, BalloonShowingEvent, handler);
    }

    internal static RoutedEventArgs RaiseBalloonShowingEvent(DependencyObject target, TaskbarIcon source)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs(BalloonShowingEvent, source);
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static void AddBalloonClosingHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.AddHandler(element, BalloonClosingEvent, handler);
    }

    public static void RemoveBalloonClosingHandler(DependencyObject element, RoutedEventHandler handler)
    {
        RoutedEventHelper.RemoveHandler(element, BalloonClosingEvent, handler);
    }

    internal static RoutedEventArgs RaiseBalloonClosingEvent(DependencyObject target, TaskbarIcon source)
    {
        if (target == null)
        {
            return null;
        }

        var routedEventArgs = new RoutedEventArgs(BalloonClosingEvent, source);
        RoutedEventHelper.RaiseEvent(target, routedEventArgs);
        return routedEventArgs;
    }

    public static TaskbarIcon GetParentTaskbarIcon(DependencyObject d)
    {
        return (TaskbarIcon)d.GetValue(ParentTaskbarIconProperty);
    }

    public static void SetParentTaskbarIcon(DependencyObject d, TaskbarIcon value)
    {
        d.SetValue(ParentTaskbarIconProperty, value);
    }

    static TaskbarIcon()
    {
        TrayPopupResolvedPropertyKey = DependencyProperty.RegisterReadOnly("TrayPopupResolved", typeof(Popup), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        TrayPopupResolvedProperty = TrayPopupResolvedPropertyKey.DependencyProperty;
        TrayToolTipResolvedPropertyKey = DependencyProperty.RegisterReadOnly("TrayToolTipResolved", typeof(ToolTip), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        TrayToolTipResolvedProperty = TrayToolTipResolvedPropertyKey.DependencyProperty;
        CustomBalloonPropertyKey = DependencyProperty.RegisterReadOnly("CustomBalloon", typeof(Popup), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        CustomBalloonProperty = CustomBalloonPropertyKey.DependencyProperty;
        IconSourceProperty = DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null, IconSourcePropertyChanged));
        ToolTipTextProperty = DependencyProperty.Register("ToolTipText", typeof(string), typeof(TaskbarIcon), new FrameworkPropertyMetadata(string.Empty, ToolTipTextPropertyChanged));
        TrayToolTipProperty = DependencyProperty.Register("TrayToolTip", typeof(UIElement), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null, TrayToolTipPropertyChanged));
        TrayPopupProperty = DependencyProperty.Register("TrayPopup", typeof(UIElement), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null, TrayPopupPropertyChanged));
        MenuActivationProperty = DependencyProperty.Register("MenuActivation", typeof(PopupActivationMode), typeof(TaskbarIcon), new FrameworkPropertyMetadata(PopupActivationMode.RightClick));
        PopupActivationProperty = DependencyProperty.Register("PopupActivation", typeof(PopupActivationMode), typeof(TaskbarIcon), new FrameworkPropertyMetadata(PopupActivationMode.LeftClick));
        DoubleClickCommandProperty = DependencyProperty.Register("DoubleClickCommand", typeof(ICommand), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        DoubleClickCommandParameterProperty = DependencyProperty.Register("DoubleClickCommandParameter", typeof(object), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        DoubleClickCommandTargetProperty = DependencyProperty.Register("DoubleClickCommandTarget", typeof(IInputElement), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        LeftClickCommandProperty = DependencyProperty.Register("LeftClickCommand", typeof(ICommand), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        LeftClickCommandParameterProperty = DependencyProperty.Register("LeftClickCommandParameter", typeof(object), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        LeftClickCommandTargetProperty = DependencyProperty.Register("LeftClickCommandTarget", typeof(IInputElement), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null));
        TrayLeftMouseDownEvent = EventManager.RegisterRoutedEvent("TrayLeftMouseDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayRightMouseDownEvent = EventManager.RegisterRoutedEvent("TrayRightMouseDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayMiddleMouseDownEvent = EventManager.RegisterRoutedEvent("TrayMiddleMouseDown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayLeftMouseUpEvent = EventManager.RegisterRoutedEvent("TrayLeftMouseUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayRightMouseUpEvent = EventManager.RegisterRoutedEvent("TrayRightMouseUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayMiddleMouseUpEvent = EventManager.RegisterRoutedEvent("TrayMiddleMouseUp", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("TrayMouseDoubleClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayMouseMoveEvent = EventManager.RegisterRoutedEvent("TrayMouseMove", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayBalloonTipShownEvent = EventManager.RegisterRoutedEvent("TrayBalloonTipShown", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayBalloonTipClosedEvent = EventManager.RegisterRoutedEvent("TrayBalloonTipClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayBalloonTipClickedEvent = EventManager.RegisterRoutedEvent("TrayBalloonTipClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayContextMenuOpenEvent = EventManager.RegisterRoutedEvent("TrayContextMenuOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        PreviewTrayContextMenuOpenEvent = EventManager.RegisterRoutedEvent("PreviewTrayContextMenuOpen", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayPopupOpenEvent = EventManager.RegisterRoutedEvent("TrayPopupOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        PreviewTrayPopupOpenEvent = EventManager.RegisterRoutedEvent("PreviewTrayPopupOpen", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayToolTipOpenEvent = EventManager.RegisterRoutedEvent("TrayToolTipOpen", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        PreviewTrayToolTipOpenEvent = EventManager.RegisterRoutedEvent("PreviewTrayToolTipOpen", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        TrayToolTipCloseEvent = EventManager.RegisterRoutedEvent("TrayToolTipClose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        PreviewTrayToolTipCloseEvent = EventManager.RegisterRoutedEvent("PreviewTrayToolTipClose", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        PopupOpenedEvent = EventManager.RegisterRoutedEvent("PopupOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        ToolTipOpenedEvent = EventManager.RegisterRoutedEvent("ToolTipOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        ToolTipCloseEvent = EventManager.RegisterRoutedEvent("ToolTipClose", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        BalloonShowingEvent = EventManager.RegisterRoutedEvent("BalloonShowing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        BalloonClosingEvent = EventManager.RegisterRoutedEvent("BalloonClosing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TaskbarIcon));
        ParentTaskbarIconProperty = DependencyProperty.RegisterAttached("ParentTaskbarIcon", typeof(TaskbarIcon), typeof(TaskbarIcon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
        var typeMetadata = new PropertyMetadata(Visibility.Visible, VisibilityPropertyChanged);
        UIElement.VisibilityProperty.OverrideMetadata(typeof(TaskbarIcon), typeMetadata);
        typeMetadata = new FrameworkPropertyMetadata(DataContextPropertyChanged);
        FrameworkElement.DataContextProperty.OverrideMetadata(typeof(TaskbarIcon), typeMetadata);
        typeMetadata = new FrameworkPropertyMetadata(ContextMenuPropertyChanged);
        FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof(TaskbarIcon), typeMetadata);
    }
}
