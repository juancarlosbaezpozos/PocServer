using Principal.Controls.Core;
using Principal.Server.Check;
using Principal.Server.Controller.Images;
using Principal.Server.Objects;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Principal.Server.Controller.Check;

[DesignTimeVisible(false)]
public sealed partial class StiCheckControl : UserControl, IComponentConnector
{
    private readonly StiCheckStatusControl checksControl;

    internal StiCheck check;

    internal static bool Lock;

    public StiCheckControl(StiCheck check, int number, StiCheckStatusControl csControl)
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            DoScaling();
            buttonCopyToClipboard.ToolTip = "Copiar al Portapapeles";
            this.check = check;
            checksControl = csControl;
            textBlockNumber.Text = $"{number}.";
            textBlockShortMessage.Text = check.ShortMessage;
            textBlockLongMessage.Text = check.LongMessage;
            imageView.Visibility = ((!check.PreviewVisible) ? Visibility.Collapsed : Visibility.Visible);
            if (check.PreviewVisible)
            {
                imageView.ToolTipOpening += ImageView_ToolTipOpening;
            }
            switch (check.ServerStatus)
            {
                case StiCheckServerStatus.Error:
                    imageMessage.Source = StiControllerImages.Items.Error(StiImageSize.OneHalf);
                    textBlockNumber.Text += "Error: ";
                    break;
                case StiCheckServerStatus.Information:
                    imageMessage.Source = StiControllerImages.Items.Information(StiImageSize.OneHalf);
                    textBlockNumber.Text += "Información: ";
                    break;
                case StiCheckServerStatus.Warning:
                    imageMessage.Source = StiControllerImages.Items.Warning(StiImageSize.OneHalf);
                    textBlockNumber.Text += "Advertencia: ";
                    break;
            }
            for (var num = check.Actions.Count - 1; num >= 0; num--)
            {
                var stiServerCheckerAction = check.Actions[num];
                var stiButton = new StiButton
                {
                    MinWidth = 85.0,
                    Padding = new Thickness(6.0, 0.0, 6.0, 0.0),
                    Margin = new Thickness(4.0),
                    Content = stiServerCheckerAction.Name,
                    Tag = stiServerCheckerAction
                };
                stiButton.Click += ActionButton_Click;
                stiButton.ToolTip = stiServerCheckerAction.Description;
                stackPanelActions.Children.Insert(0, stiButton);
            }
        }
    }

    private void DoScaling()
    {
        imageView.Source = StiControllerImages.Items.View();
        imageCopy.Source = StiControllerImages.Items.Copy();
    }

    private static void CreateImage(StiCheck check, out BitmapSource pixelElement, out BitmapSource pixelHighlightedElement)
    {
        pixelElement = null;
        pixelHighlightedElement = null;
        _ = check.PreviewVisible;
    }

    private void ButtonCopyToClipboard_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Clipboard.SetDataObject(check.ShortMessage + "\r\n" + check.LongMessage, copy: true);
        }
        catch
        {
        }
    }

    private void ImageView_ToolTipOpening(object sender, ToolTipEventArgs e)
    {
        e.Handled = true;
        imageView.ToolTipOpening -= ImageView_ToolTipOpening;
        if (gridViewImage.Source != null || gridViewHighlightedImage.Source != null)
        {
            return;
        }
        base.Cursor = Cursors.Wait;
        try
        {
            CreateImage(check, out var pixelElement, out var pixelHighlightedElement);
            gridViewImage.Source = null;
            if (pixelElement != null)
            {
                gridViewImage.Source = pixelElement;
            }
            else
            {
                imageView.Visibility = Visibility.Collapsed;
            }
            gridViewHighlightedImage.Source = null;
            if (pixelHighlightedElement != null)
            {
                gridViewHighlightedImage.Source = pixelHighlightedElement;
                gridViewHighlightedImage.Visibility = Visibility.Visible;
            }
            else
            {
                gridViewHighlightedImage.Visibility = Visibility.Collapsed;
            }
        }
        finally
        {
            base.Cursor = null;
        }
    }

    private void ActionButton_Click(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (sender is Button button)
        {
            var stiServerCheckerAction = button.Tag as StiServerCheckerAction;
            stiServerCheckerAction?.Invoke(check.Element, check.ElementName);
            if (stiServerCheckerAction == null || stiServerCheckerAction.RemoveCheckAfterInvokeAction)
            {
                checksControl.RemoveCheckControl(this);
            }
        }
    }
}
