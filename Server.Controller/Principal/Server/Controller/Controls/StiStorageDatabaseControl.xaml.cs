using System.Threading;
using System.Threading.Tasks;
using Principal.Controls.Core;
using Principal.Server.Controller.Images;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Principal.Server.Controller.Controls;

public partial class StiStorageDatabaseControl : UserControl, IComponentConnector
{
    private StiStorageControl storageControl;

    public string ConnectionString
    {
        get => textBoxConnectionString.Text;
        set => textBoxConnectionString.Text = value;
    }

    public bool IsChangedConnection { get; set; }

    public StiStorageControl StorageControl
    {
        get => storageControl;
        set => storageControl = value;
    }

    public StiStorageDatabaseControl()
    {
        InitializeComponent();
        DoScaling();
    }

    private void DoScaling()
    {
        imageCut.Source = StiControllerImages.Items.Cut();
        imageCopy.Source = StiControllerImages.Items.Copy();
        imagePaste.Source = StiControllerImages.Items.Paste();
        imageButtonClean.Source = StiControllerImages.Items.ClearContent();
        imageButtonTest.Source = StiControllerImages.Items.Check();
    }

    private void ShowTestConnectionProgress()
    {
        imageButtonTest.Opacity = 0.3;
        progressTestConnection.Visibility = Visibility.Visible;
        progressTestConnection.IsActive = true;
    }

    private void HideTestConnectionProgress()
    {
        progressTestConnection.IsActive = false;
        progressTestConnection.Visibility = Visibility.Collapsed;
        imageButtonTest.Opacity = 1.0;
    }

    public bool CheckSaveButton()
    {
        bool result = true;

        return result;
    }

    public void Save()
    {

    }

    private void TextBoxConnectionString_TextChanged(object sender, TextChangedEventArgs e)
    {
        IsChangedConnection = true;
        storageControl.CheckStateSaveButton();
    }

    private void TextBoxConnectionString_SelectionChanged(object sender, RoutedEventArgs e)
    {
        StiMenuItem stiMenuItem = menuItemCopy;
        bool isEnabled = (menuItemCut.IsEnabled = !string.IsNullOrEmpty(textBoxConnectionString.SelectedText));
        stiMenuItem.IsEnabled = isEnabled;
    }

    private void MenuItemCut_Click(object sender, RoutedEventArgs e)
    {
        textBoxConnectionString.Cut();
    }

    private void MenuItemCopy_Click(object sender, RoutedEventArgs e)
    {
        textBoxConnectionString.Copy();
    }

    private void MenuItemPaste_Click(object sender, RoutedEventArgs e)
    {
        textBoxConnectionString.Paste();
    }

    private void ButtonDatabaseClean_Click(object sender, RoutedEventArgs e)
    {
        textBoxConnectionString.Text = string.Empty;
    }

    private void ButtonTestConnection_Click(object sender, RoutedEventArgs e)
    {
        base.IsEnabled = false;
        ShowTestConnectionProgress();

        //try
        //{
            Task.Run(() =>
            {
                Thread.Sleep(5000);
            }).ContinueWith((task) =>
            {
                Dispatcher.Invoke(() =>
                {
                    HideTestConnectionProgress();
                    base.IsEnabled = true;
                });
            });
        //}
        //finally
        //{
        //    base.IsEnabled = true;
        //    HideTestConnectionProgress();
        //}
    }
}
