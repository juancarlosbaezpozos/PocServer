using Principal.Server.Check;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Principal.Server.Controller.Check;

[DesignTimeVisible(false)]
public sealed partial class StiCheckStatusControl : UserControl, IComponentConnector
{
    internal StiServerControl serverControl;

    public event EventHandler Process;

    public StiCheckStatusControl()
    {
        InitializeComponent();
        if (!DesignerProperties.GetIsInDesignMode(this))
        {
            textBlockNoIssues.Visibility = Visibility.Visible;
            listBoxItems.Visibility = Visibility.Collapsed;
        }
    }

    private void InvokeProcess()
    {
        this.Process?.Invoke(this, EventArgs.Empty);
    }

    private Task CheckServerAsync(BuildCheckDelegate BuildCheck)
    {
        textBlockNoIssues.Visibility = Visibility.Collapsed;
        return Task.Run(delegate
        {
            new StiCheckEngine().Check(BuildCheck);
            base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
            {
                textBlockNoIssues.Visibility = ((listBoxItems.ItemsSource != null) ? Visibility.Collapsed : Visibility.Visible);
            });
        });
    }

    public async Task BuildChecksAsync()
    {
        InvokeProcess();
        ClearResults();
        await CheckServerAsync(BuildCheck);
    }

    private void BuildCheck(StiCheck check)
    {
        if (check.Actions != null || check.Actions.Count > 0)
        {
            foreach (var action in check.Actions)
            {
                if (action is StiStartServiceAction)
                {
                    action.OnAfterAction += delegate
                    {
                        serverControl.tabControl.SelectedItem = serverControl.pageWindowsServices;
                    };
                }
                if (action is StiInstallServiceAction)
                {
                    action.OnAfterAction += delegate
                    {
                        serverControl.tabControl.SelectedItem = serverControl.pageWindowsServices;
                    };
                }
            }
        }
        base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Action)delegate
        {
            var observableCollection = listBoxItems.ItemsSource as ObservableCollection<StiCheckControl>;
            if (observableCollection == null)
            {
                observableCollection = new ObservableCollection<StiCheckControl>();
                listBoxItems.ItemsSource = observableCollection;
            }
            observableCollection.Add(new StiCheckControl(check, observableCollection.Count, this)
            {
                Margin = new Thickness(0.0, 4.0, 0.0, 4.0)
            });
            listBoxItems.Visibility = Visibility.Visible;
        });
    }

    public void ClearResults()
    {
        listBoxItems.ItemsSource = null;
        listBoxItems.Visibility = Visibility.Collapsed;
        textBlockNoIssues.Visibility = Visibility.Visible;
    }

    internal void RemoveCheckControl(StiCheckControl control)
    {
        if (listBoxItems.ItemsSource is ObservableCollection<StiCheckControl> observableCollection)
        {
            if (observableCollection != null && observableCollection.Contains(control))
            {
                observableCollection.Remove(control);
            }
            control.IsEnabled = false;
        }
    }
}
