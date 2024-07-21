using Principal.Server.Controller.Images;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Principal.Server.Controller.Controls;

public partial class StiCheckerControl : UserControl, IComponentConnector
{
    public StiCheckerControl(StiServerControl serverControl)
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            checkStatusControl.listBoxItems.Margin = new Thickness(0.0, 0.0, 8.0, 8.0);
            checkStatusControl.serverControl = serverControl;
            DoScaling();
            base.Loaded += OnLoaded;
        }
    }

    private void DoScaling()
    {
        imageRun.Source = StiControllerImages.Items.Start();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        base.Loaded -= OnLoaded;
        checkStatusControl.progress.IsActive = true;
        buttonCheckServer.IsEnabled = false;
        await Task.Delay(10);
        await checkStatusControl.BuildChecksAsync();
        buttonCheckServer.IsEnabled = true;
        checkStatusControl.progress.IsActive = false;
    }

    private async void buttonCheckServerClick(object sender, RoutedEventArgs e)
    {
        checkStatusControl.progress.IsActive = true;
        await Task.Delay(30);
        buttonCheckServer.IsEnabled = false;
        await checkStatusControl.BuildChecksAsync();
        buttonCheckServer.IsEnabled = true;
        checkStatusControl.progress.IsActive = false;
    }
}
