using System;
using System.Runtime.InteropServices;

namespace Principal.Server.Controller.NotifyIcon.Interop;

internal static class WinApi
{
    [DllImport("shell32.Dll", CharSet = CharSet.Unicode)]
    public static extern bool Shell_NotifyIcon(NotifyCommand cmd, [In] ref NotifyIconData data);

    [DllImport("USER32.DLL", EntryPoint = "CreateWindowExW", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("USER32.DLL")]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wparam, IntPtr lparam);

    [DllImport("USER32.DLL", EntryPoint = "RegisterClassW", SetLastError = true)]
    public static extern short RegisterClass(ref WindowClass lpWndClass);

    [DllImport("User32.Dll", EntryPoint = "RegisterWindowMessageW")]
    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport("USER32.DLL", SetLastError = true)]
    public static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("USER32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern int GetDoubleClickTime();

    [DllImport("USER32.DLL", SetLastError = true)]
    public static extern bool GetPhysicalCursorPos(ref Point lpPoint);

    [DllImport("USER32.DLL", SetLastError = true)]
    public static extern bool GetCursorPos(ref Point lpPoint);
}
