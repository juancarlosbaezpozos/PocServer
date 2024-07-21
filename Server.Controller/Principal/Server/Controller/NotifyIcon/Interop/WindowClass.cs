using System;
using System.Runtime.InteropServices;

namespace Principal.Server.Controller.NotifyIcon.Interop;

public struct WindowClass
{
    internal uint style;

    internal WindowProcedureHandler lpfnWndProc;

    public int cbClsExtra;

    public int cbWndExtra;

    public IntPtr hInstance;

    public IntPtr hIcon;

    public IntPtr hCursor;

    public IntPtr hbrBackground;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszMenuName;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszClassName;
}
