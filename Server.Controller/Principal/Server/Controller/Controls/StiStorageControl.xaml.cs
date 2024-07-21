using Principal.Server.Controller.Images;
using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Principal.Server.Controller.Controls;

public partial class StiStorageControl : UserControl, IComponentConnector
{
    public StiServerControl ServerControl { get; set; }

    public StiStorageControl()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            storageDatabase.StorageControl = this;
            DoScaling();
        }
    }

    private void DoScaling()
    {
        imageRestart.Source = StiControllerImages.Items.Warning(StiImageSize.Double);
    }

    public void CheckStateSaveButton()
    {
        buttonSaveDatabase.IsEnabled = storageDatabase.CheckSaveButton();
    }

    private void ButtonSaveDatabase_Click(object sender, RoutedEventArgs e)
    {
        storageDatabase.Save();
        buttonSaveDatabase.IsEnabled = false;
        panelRestart.Visibility = Visibility.Visible;
    }

    private async void ButtonRestart_Click(object sender, RoutedEventArgs e)
    {
        panelRestart.Visibility = Visibility.Collapsed;
        ServerControl.tabControl.SelectedIndex = 0;
        await ServerControl.serviceInstallation.InvokeRestartServiceAsync();
    }
}
