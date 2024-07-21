using System;

namespace Principal.Server.Controller.NotifyIcon.Interop;

internal delegate IntPtr WindowProcedureHandler(IntPtr hwnd, uint uMsg, IntPtr wparam, IntPtr lparam);
