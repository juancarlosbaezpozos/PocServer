using System.Drawing;

namespace Principal.Server.Controller.NotifyIcon.Interop;

public static class TrayInfo
{
    public static Point GetTrayLocation()
    {
        var appBarInfo = new AppBarInfo();
        appBarInfo.GetSystemTaskBarPosition();
        var workArea = appBarInfo.WorkArea;
        var x = 0;
        var y = 0;
        if (appBarInfo.Edge == AppBarInfo.ScreenEdge.Left)
        {
            x = workArea.Left + 2;
            y = workArea.Bottom;
        }
        else if (appBarInfo.Edge == AppBarInfo.ScreenEdge.Bottom)
        {
            x = workArea.Right;
            y = workArea.Bottom;
        }
        else if (appBarInfo.Edge == AppBarInfo.ScreenEdge.Top)
        {
            x = workArea.Right;
            y = workArea.Top;
        }
        else if (appBarInfo.Edge == AppBarInfo.ScreenEdge.Right)
        {
            x = workArea.Right;
            y = workArea.Bottom;
        }
        var result = default(Point);
        result.X = x;
        result.Y = y;
        return result;
    }
}
