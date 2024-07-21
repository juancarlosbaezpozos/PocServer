using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

namespace Principal.Controls
{
    [SuppressUnmanagedCodeSecurity]
    [SecuritySafeCritical]
    public static class RibbonWinApi
    {
        public class MonitorInfo
        {
            public string DeviceID;

            public string DeviceName;

            public DisplayDeviceStateFlags StateFlags;

            public MonitorInfo(string deviceID, string deviceName, DisplayDeviceStateFlags stateFlags)
            {
                DeviceID = deviceID.ToLowerInvariant();
                DeviceName = deviceName.ToLowerInvariant();
                StateFlags = stateFlags;
            }

            public override string ToString()
            {
                return $"DeviceID={DeviceID}, DeviceName={DeviceName}";
            }
        }

        [Flags]
        public enum DisplayDeviceStateFlags
        {
            AttachedToDesktop = 1,
            MultiDriver = 2,
            PrimaryDevice = 4,
            MirroringDriver = 8,
            VGACompatible = 0x16,
            Removable = 0x20,
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;

            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_LAST
        }

        public struct WINDOWPOS
        {
            public IntPtr hwnd;

            public IntPtr hwndInsertAfter;

            public int x;

            public int y;

            public int cx;

            public int cy;

            public int flags;
        }

        public struct NCCALCSIZE_PARAMS
        {
            public RECT rgrc0;

            public RECT rgrc1;

            public RECT rgrc2;

            public IntPtr lppos;
        }

        public struct RECT
        {
            public int Left;

            public int Top;

            public int Right;

            public int Bottom;

            public int Width => Right - Left;

            public int Height => Bottom - Top;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rect r)
            {
                Left = (int)Math.Ceiling(r.Left);
                Top = (int)Math.Ceiling(r.Top);
                Right = (int)Math.Ceiling(r.Right);
                Bottom = (int)Math.Ceiling(r.Bottom);
            }

            public Rect ToRectangle()
            {
                return new Rect(Left, Top, Right - Left, Bottom - Top);
            }

            public static RECT FromRectangle(Rect r)
            {
                return new RECT((int)Math.Ceiling(r.Left), (int)Math.Ceiling(r.Top), (int)Math.Ceiling(r.Right), (int)Math.Ceiling(r.Bottom));
            }
        }

        public struct POINT
        {
            public int x;

            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public struct MINMAXINFO
        {
            public POINT ptReserved;

            public POINT ptMaxSize;

            public POINT ptMaxPosition;

            public POINT ptMinTrackSize;

            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            public RECT rcMonitor;

            public RECT rcWork;

            public int dwFlags;
        }

        public const int WM_GETMINMAXINFO = 36;

        public const int WM_NCCALCSIZE = 131;

        public const int WM_NCHITTEST = 132;

        public const int WM_NCLBUTTONDOWN = 161;

        public const int HTCAPTION = 2;

        public const int HTLEFT = 10;

        public const int HTRIGHT = 11;

        public const int HTTOP = 12;

        public const int HTBOTTOM = 15;

        public const int HTTOPLEFT = 13;

        public const int HTTOPRIGHT = 14;

        public const int HTBOTTOMLEFT = 16;

        public const int HTBOTTOMRIGHT = 17;

        public const int HTMINBUTTON = 8;

        public const int HTMAXBUTTON = 9;

        public const int HTCLOSE = 20;

        public const int HTHELP = 21;

        public const int HTREDUCE = 8;

        public const int MONITOR_DEFAULTTONEAREST = 2;

        public const int GWL_EXSTYLE = -20;

        public const int WS_EX_DLGMODALFRAME = 1;

        public const int SWP_NOSIZE = 1;

        public const int SWP_NOMOVE = 2;

        public const int SWP_NOZORDER = 4;

        public const int SWP_FRAMECHANGED = 32;

        public const int WVR_VALIDRECTS = 1024;

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("Gdi32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, uint newStyle);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern bool SystemParametersInfo(int nAction, int nParam, ref int value, int ignore);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmDefWindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, out IntPtr plResult);

        [DllImport("dwmapi.dll")]
        internal static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttributeToGet, out RECT pvAttributeValue, int cbAttribute);
    }
}
