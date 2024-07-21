using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;

namespace Principal.Controls
{
    [SuppressUnmanagedCodeSecurity]
    [SecuritySafeCritical]
    public static class IconHelper
    {
        private const int GWL_EXSTYLE = -20;

        private const int WS_EX_DLGMODALFRAME = 1;

        private const int SWP_NOSIZE = 1;

        private const int SWP_NOMOVE = 2;

        private const int SWP_NOZORDER = 4;

        private const int SWP_FRAMECHANGED = 32;

        private const uint WM_SETICON = 128u;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void RemoveIcon(Window window)
        {
            IntPtr handle = new WindowInteropHelper(window).Handle;
            int windowLong = GetWindowLong(handle, -20);
            SetWindowLong(handle, -20, windowLong | 1);
            SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
        }
    }
}
