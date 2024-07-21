using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Principal.Server.Objects;

namespace Principal.Server.Controller.NotifyIcon.Interop;

internal class AppBarInfo
{
    public enum ScreenEdge
    {
        Undefined = -1,
        Left,
        Top,
        Right,
        Bottom
    }

    private struct APPBARDATA
    {
        public uint cbSize;

        public IntPtr hWnd;

        public uint uCallbackMessage;

        public uint uEdge;

        public RECT rc;

        public int lParam;
    }

    private struct RECT
    {
        public int left;

        public int top;

        public int right;

        public int bottom;
    }

    private const int ABE_BOTTOM = 3;

    private const int ABE_LEFT = 0;

    private const int ABE_RIGHT = 2;

    private const int ABE_TOP = 1;

    private const int ABM_GETTASKBARPOS = 5;

    private const uint SPI_GETWORKAREA = 48u;

    private APPBARDATA m_data;

    public ScreenEdge Edge => (ScreenEdge)m_data.uEdge;

    public Rectangle WorkArea
    {
        get
        {
            RECT rECT = default(RECT);
            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(rECT));
            int num = SystemParametersInfo(48u, 0u, intPtr, 0u);
            rECT = (RECT)Marshal.PtrToStructure(intPtr, rECT.GetType());
            if (num == 1)
            {
                Marshal.FreeHGlobal(intPtr);
                return new Rectangle(rECT.left, rECT.top, rECT.right - rECT.left, rECT.bottom - rECT.top);
            }
            return new Rectangle(0, 0, 0, 0);
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("shell32.dll")]
    private static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA data);

    [DllImport("user32.dll")]
    private static extern int SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

    public void GetPosition(string strClassName, string strWindowName)
    {
        m_data = default(APPBARDATA);
        m_data.cbSize = (uint)Marshal.SizeOf(m_data.GetType());
        if (FindWindow(strClassName, strWindowName) != IntPtr.Zero)
        {
            if (SHAppBarMessage(5u, ref m_data) != 1)
            {
                throw new Exception("Failed to communicate with the given AppBar");
            }
            return;
        }
        throw new Exception("Failed to find an AppBar that matched the given criteria");
    }

    public void GetSystemTaskBarPosition()
    {
        GetPosition("Shell_TrayWnd", null);
    }
}
