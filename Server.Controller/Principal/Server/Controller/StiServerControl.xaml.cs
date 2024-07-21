using Principal.Server.Controller.Controls;
using Principal.Server.Controller.Images;
using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Principal.Server.Controller;

public partial class StiServerControl : UserControl, IComponentConnector
{
    public StiManagerWindow ManagerWindow { get; set; }

    public StiServerControl()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            storageControl.ServerControl = this;
            DoScaling();
        }
    }

    private void DoScaling()
    {
        imageTabStorage.Source = StiControllerImages.Items.ServerStorage(StiImageSize.Double);
        imageTabCheck.Source = StiControllerImages.Items.ServerCheck(StiImageSize.Double);
        imageTabService.Source = StiControllerImages.Items.ServerService(StiImageSize.Double);
    }

    private void PageCheck_Selected(object sender, RoutedEventArgs e)
    {
        pageCheck.Selected -= PageCheck_Selected;
        pageCheck.Content = new StiCheckerControl(this);
    }
}
