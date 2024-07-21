using Principal.Server.Objects;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class StiListBox : ListBox, IStiWpfV2Control
    {
        public event EventHandler ItemDeleteClick;

        public StiListBox()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                if (StiOSVersion.IsWindows11())
                {
                    base.BorderThickness = new Thickness(StiScale.ScaleThickness);
                }
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiListBox;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StiListBoxItem();
        }

        internal void InvokeItemDeleteClick(StiListBoxItem item)
        {
            this.ItemDeleteClick?.Invoke(item, EventArgs.Empty);
        }
    }
}
