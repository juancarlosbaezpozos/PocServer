using Principal.Server.Objects;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiCheckBox : CheckBox, IStiWpfV2Control
    {
        public virtual StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Win11;

        public event EventHandler CheckedChanged;

        public StiCheckBox()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
                if (StiOSVersion.IsWindows11())
                {
                    base.BorderThickness = new Thickness(StiScale.ScaleThickness);
                }
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiCheckBox;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            InvokeCheckedChanged();
        }

        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            InvokeCheckedChanged();
        }

        private void InvokeCheckedChanged()
        {
            this.CheckedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
