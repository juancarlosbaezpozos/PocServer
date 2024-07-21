using Principal.Controls.Core;
using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;

namespace Principal.Server.Controller;

public partial class StiManagerWindow : StiWindow, IComponentConnector
{
    public StiManagerWindow()
    {
        InitializeComponent();
        if (DesignerProperties.GetIsInDesignMode(this))
        {
            return;
        }
        serverControl.ManagerWindow = this;
        serverControl.separatorFrequency.Visibility = Visibility.Collapsed;
        var workingArea = Screen.PrimaryScreen.WorkingArea;
        base.Left = (double)workingArea.Right / StiScale.X - base.Width + 5.0;
        base.Top = (double)workingArea.Bottom / StiScale.Y - base.Height + 5.0;
    }
}
