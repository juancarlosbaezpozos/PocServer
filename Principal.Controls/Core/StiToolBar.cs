using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class StiToolBar : ToolBar, IStiWpfV2Control
    {
        private Style ToggleButtonStyleEx;

        private Style ButtonStyleEx;

        private Style RadioButtonStyleEx;

        private Style CheckBoxStyleEx;

        private Style TextBoxStyleEx;

        private Style MenuStyleEx;

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(StiToolBar));

        public StiWpfV2ThemeMode ThemeMode => StiWpfV2ThemeMode.Win11;

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public StiToolBar()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this, ThemeMode);
                StiWpfSkinHelper.SkinChanged += SkinHelper_SkinChanged;
                SkinHelper_SkinChanged(null, null);
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiToolBar;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            if (!(element is FrameworkElement frameworkElement))
            {
                return;
            }
            Type type = frameworkElement.GetType();
            if (type == typeof(StiToggleButton))
            {
                frameworkElement.SetValue(FrameworkElement.StyleProperty, ToggleButtonStyleEx);
            }
            else if (type == typeof(StiButton))
            {
                StiButton stiButton = (StiButton)frameworkElement;
                if (stiButton.ApplyStyleInToolbar)
                {
                    frameworkElement.SetValue(FrameworkElement.StyleProperty, ButtonStyleEx);
                }
            }
            else if (type == typeof(StiRadioButton))
            {
                frameworkElement.SetValue(FrameworkElement.StyleProperty, RadioButtonStyleEx);
            }
            else if (type == typeof(StiCheckBox))
            {
                frameworkElement.SetValue(FrameworkElement.StyleProperty, CheckBoxStyleEx);
            }
            else if (type == typeof(StiTextBox))
            {
                frameworkElement.SetValue(FrameworkElement.StyleProperty, TextBoxStyleEx);
            }
            else if (type == typeof(StiMenu))
            {
                frameworkElement.SetValue(FrameworkElement.StyleProperty, MenuStyleEx);
            }
        }

        private void SkinHelper_SkinChanged(object sender, EventArgs e)
        {
            ResourceDictionary dictionary = StiWpfV2ThemeHelper.GetDictionary(GetV2ID());
            if (dictionary != null)
            {
                ToggleButtonStyleEx = dictionary["StiToolBarToggleButtonStyle"] as Style;
                ButtonStyleEx = dictionary["StiToolBarButtonStyle"] as Style;
                RadioButtonStyleEx = dictionary["StiToolBarRadioButtonStyle"] as Style;
                CheckBoxStyleEx = dictionary["StiToolBarCheckBoxStyle"] as Style;
                TextBoxStyleEx = dictionary["StiToolBarTextBoxStyle"] as Style;
                MenuStyleEx = dictionary["StiToolBarMenuStyle"] as Style;
            }
        }
    }
}
