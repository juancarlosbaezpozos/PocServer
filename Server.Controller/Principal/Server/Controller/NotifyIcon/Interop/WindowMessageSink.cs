using System;
using System.ComponentModel;

namespace Principal.Server.Controller.NotifyIcon.Interop;

public class WindowMessageSink : IDisposable
{
    public const int CallbackMessageId = 1024;

    private uint taskbarRestartMessageId;

    private bool isDoubleClick;

    private WindowProcedureHandler messageHandler;

    internal string WindowId { get; private set; }

    internal IntPtr MessageWindowHandle { get; private set; }

    public NotifyIconVersion Version { get; set; }

    public bool IsDisposed { get; private set; }

    public event Action<bool> ChangeToolTipStateRequest;

    public event Action<MouseEvent> MouseEventReceived;

    public event Action<bool> BalloonToolTipChanged;

    public event Action TaskbarCreated;

    public WindowMessageSink(NotifyIconVersion version)
    {
        Version = version;
        CreateMessageWindow();
    }

    private WindowMessageSink()
    {
    }

    internal static WindowMessageSink CreateEmpty()
    {
        return new WindowMessageSink
        {
            MessageWindowHandle = IntPtr.Zero,
            Version = NotifyIconVersion.Vista
        };
    }

    private void CreateMessageWindow()
    {
        WindowId = "WPFTaskbarIcon_" + DateTime.Now.Ticks;
        messageHandler = OnWindowMessageReceived;
        WindowClass lpWndClass = default(WindowClass);
        lpWndClass.style = 0u;
        lpWndClass.lpfnWndProc = messageHandler;
        lpWndClass.cbClsExtra = 0;
        lpWndClass.cbWndExtra = 0;
        lpWndClass.hInstance = IntPtr.Zero;
        lpWndClass.hIcon = IntPtr.Zero;
        lpWndClass.hCursor = IntPtr.Zero;
        lpWndClass.hbrBackground = IntPtr.Zero;
        lpWndClass.lpszMenuName = "";
        lpWndClass.lpszClassName = WindowId;
        WinApi.RegisterClass(ref lpWndClass);
        taskbarRestartMessageId = WinApi.RegisterWindowMessage("TaskbarCreated");
        MessageWindowHandle = WinApi.CreateWindowEx(0, WindowId, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        if (MessageWindowHandle == IntPtr.Zero)
        {
            throw new Win32Exception("Message window handle was not a valid pointer");
        }
    }

    private IntPtr OnWindowMessageReceived(IntPtr hwnd, uint messageId, IntPtr wparam, IntPtr lparam)
    {
        if (messageId == taskbarRestartMessageId)
        {
            this.TaskbarCreated();
        }
        ProcessWindowMessage(messageId, wparam, lparam);
        return WinApi.DefWindowProc(hwnd, messageId, wparam, lparam);
    }

    private void ProcessWindowMessage(uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg != 1024)
        {
            return;
        }
        switch (lParam.ToInt32())
        {
            case 512:
                this.MouseEventReceived(MouseEvent.MouseMove);
                break;
            case 513:
                this.MouseEventReceived(MouseEvent.IconLeftMouseDown);
                break;
            case 514:
                if (!isDoubleClick)
                {
                    this.MouseEventReceived(MouseEvent.IconLeftMouseUp);
                }
                isDoubleClick = false;
                break;
            case 515:
                isDoubleClick = true;
                this.MouseEventReceived(MouseEvent.IconDoubleClick);
                break;
            case 516:
                this.MouseEventReceived(MouseEvent.IconRightMouseDown);
                break;
            case 517:
                this.MouseEventReceived(MouseEvent.IconRightMouseUp);
                break;
            case 519:
                this.MouseEventReceived(MouseEvent.IconMiddleMouseDown);
                break;
            case 520:
                this.MouseEventReceived(MouseEvent.IconMiddleMouseUp);
                break;
            case 1026:
                this.BalloonToolTipChanged(obj: true);
                break;
            case 1027:
            case 1028:
                this.BalloonToolTipChanged(obj: false);
                break;
            case 1029:
                this.MouseEventReceived(MouseEvent.BalloonToolTipClicked);
                break;
            case 1030:
                this.ChangeToolTipStateRequest(obj: true);
                break;
            case 1031:
                this.ChangeToolTipStateRequest(obj: false);
                break;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~WindowMessageSink()
    {
        Dispose(disposing: false);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            IsDisposed = true;
            WinApi.DestroyWindow(MessageWindowHandle);
            messageHandler = null;
        }
    }
}
