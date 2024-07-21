using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Principal.Controls
{
    public static class StiWpfV2ThemeHelper
    {
        private class StiV2ResourceDictionary : ResourceDictionary
        {
        }

        public static class Resources
        {
            private static ResourceDictionary globalResources;

            private static Style demoViewerToolbarStyle;

            private static Style desktopViewerToolbarStyle;

            internal static void Clean()
            {
                demoViewerToolbarStyle = null;
                globalResources = null;
                desktopViewerToolbarStyle = null;
            }

            public static object GetResource(string name)
            {
                if (globalResources == null)
                {
                    globalResources = new ResourceDictionary
                    {
                        Source = new Uri($"/Principal.Controls;component/Themes/V2/{StiWpfV2ControlID.GlobalResources}.xaml", UriKind.RelativeOrAbsolute)
                    };
                    InitResourcesForOffice2013Theme(globalResources);
                }
                return globalResources[name];
            }
        }

        private static readonly bool[] states;

        private static readonly bool[] isWin11Support;

        private static ResourceDictionary[] dicts;

        private static StiV2ResourceDictionary currentResourceDictionary;

        private static ResourceDictionary styleResourceDictionary;

        static StiWpfV2ThemeHelper()
        {
            var values = Enum.GetValues(typeof(StiWpfV2ControlID));
            states = new bool[values.Length];
            isWin11Support = new bool[values.Length];
            dicts = new ResourceDictionary[values.Length];
            var stiSkinForeground = StiWpfSkinHelper.CurrentForeground;
            var stiSkinBackground = StiWpfSkinHelper.CurrentBackground;
            if (StiWpfSkinHelper.IsWinFormsMode)
            {
                stiSkinForeground = StiSkinForeground.Blue;
                stiSkinBackground = StiSkinBackground.White;
            }
            var arg = (StiOSVersion.IsWindows11() ? "_Win11" : "");
            styleResourceDictionary = new ResourceDictionary
            {
                Source = new Uri($"/Principal.Controls;component/Themes/Skins/{stiSkinForeground}{stiSkinBackground}{arg}.xaml", UriKind.RelativeOrAbsolute)
            };
            StiWpfSkinHelper.SkinChanged += SkinHelper_SkinChanged;
        }

        internal static void InitTheme(Control control)
        {
            if (control != null && Application.Current == null)
            {
                if (currentResourceDictionary == null)
                {
                    currentResourceDictionary = new StiV2ResourceDictionary();
                }
                control.Resources.MergedDictionaries.Add(currentResourceDictionary);
            }
        }

        internal static void LoadControlTheme(IStiWpfV2Control control, StiWpfV2ThemeMode mode = StiWpfV2ThemeMode.Default)
        {
            LoadControlTheme(control.GetV2ID(), mode);
        }

        private static void LoadControlTheme(StiWpfV2ControlID id, StiWpfV2ThemeMode mode = StiWpfV2ThemeMode.Default)
        {
            if (states[(int)id])
            {
                return;
            }
            states[(int)id] = true;
            isWin11Support[(int)id] = mode == StiWpfV2ThemeMode.Win11;
            if (currentResourceDictionary == null)
            {
                if (Application.Current != null)
                {
                    foreach (var mergedDictionary in Application.Current.Resources.MergedDictionaries)
                    {
                        if (mergedDictionary is StiV2ResourceDictionary)
                        {
                            currentResourceDictionary = (StiV2ResourceDictionary)mergedDictionary;
                            break;
                        }
                    }
                    if (currentResourceDictionary == null)
                    {
                        currentResourceDictionary = new StiV2ResourceDictionary();
                        Application.Current.Resources.MergedDictionaries.Add(currentResourceDictionary);
                    }
                }
                else
                {
                    currentResourceDictionary = new StiV2ResourceDictionary();
                }
            }
            var text = id.ToString();
            if (mode == StiWpfV2ThemeMode.Win11 && StiOSVersion.IsWindows11())
            {
                text += "_Win11";
            }
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri("/Principal.Controls;component/Themes/V2/" + text + ".xaml", UriKind.RelativeOrAbsolute)
            };
            InitResourcesForOffice2013Theme(resourceDictionary);
            dicts[(int)id] = resourceDictionary;
            currentResourceDictionary.MergedDictionaries.Add(resourceDictionary);
        }

        internal static ResourceDictionary GetDictionary(StiWpfV2ControlID id)
        {
            return dicts[(int)id];
        }

        private static void InitResourcesForOffice2013Theme(ResourceDictionary dict1)
        {
            if (styleResourceDictionary == null)
            {
                var uriString = (StiOSVersion.IsWindows11() ? "/Principal.Controls;component/Themes/Skins/BlueWhite.xaml" : "/Principal.Controls;component/Themes/Skins/BlueWhite_Win11.xaml");
                styleResourceDictionary = new ResourceDictionary
                {
                    Source = new Uri(uriString, UriKind.RelativeOrAbsolute)
                };
            }
            foreach (var key in styleResourceDictionary.Keys)
            {
                if (dict1.Contains(key) && styleResourceDictionary[key] is SolidColorBrush)
                {
                    ((SolidColorBrush)dict1[key]).Color = ((SolidColorBrush)styleResourceDictionary[key]).Color;
                }
            }
            var color = (Color)styleResourceDictionary["office15ColorButtonMOBackground"];
            var color2 = (Color)styleResourceDictionary["office15ColorButtonPRBackground"];
            if (dict1.Contains("office15ButtonMOBackground"))
            {
                ((SolidColorBrush)dict1["office15ButtonMOBackground"]).Color = color;
            }
            if (dict1.Contains("office15TabItemPressedBackground"))
            {
                ((LinearGradientBrush)dict1["office15TabItemPressedBackground"]).GradientStops[0].Color = color;
            }
            if (dict1.Contains("office15ToolBarBtnMOBackground"))
            {
                ((SolidColorBrush)dict1["office15ToolBarBtnMOBackground"]).Color = color;
            }
            if (dict1.Contains("office15ButtonPRBackground"))
            {
                ((SolidColorBrush)dict1["office15ButtonPRBackground"]).Color = color2;
            }
            if (dict1.Contains("office15ToolBarBtnPRBackground"))
            {
                ((SolidColorBrush)dict1["office15ToolBarBtnPRBackground"]).Color = color2;
            }
            if (dict1.Contains("office15AppColor1"))
            {
                ((SolidColorBrush)dict1["office15AppColor1"]).Color = (Color)styleResourceDictionary["office15AppColor1"];
            }
            if (dict1.Contains("office15AppColor2"))
            {
                ((SolidColorBrush)dict1["office15AppColor2"]).Color = (Color)styleResourceDictionary["office15AppColor2"];
            }
            if (dict1.Contains("PageBorderActiveColor"))
            {
                ((SolidColorBrush)dict1["PageBorderActiveColor"]).Color = (Color)styleResourceDictionary["PageBorderActiveColor"];
            }
            if (dict1.Contains("PageBorderInactiveColor"))
            {
                ((SolidColorBrush)dict1["PageBorderInactiveColor"]).Color = (Color)styleResourceDictionary["PageBorderInactiveColor"];
            }
        }

        private static void SkinHelper_SkinChanged(object sender, EventArgs e)
        {
            var values = Enum.GetValues(typeof(StiWpfV2ControlID));
            currentResourceDictionary.BeginInit();
            currentResourceDictionary.MergedDictionaries.Clear();
            dicts = new ResourceDictionary[values.Length];
            styleResourceDictionary = null;
            var currentForeground = StiWpfSkinHelper.CurrentForeground;
            var currentBackground = StiWpfSkinHelper.CurrentBackground;
            var uriString = $"/Principal.Controls;component/Themes/Skins/{currentForeground}{currentBackground}.xaml";
            styleResourceDictionary = new ResourceDictionary
            {
                Source = new Uri(uriString, UriKind.RelativeOrAbsolute)
            };
            foreach (StiWpfV2ControlID item in values)
            {
                if (states[(int)item])
                {
                    var resourceDictionary = new ResourceDictionary();
                    if (isWin11Support[(int)item] && StiOSVersion.IsWindows11())
                    {
                        resourceDictionary.Source = new Uri($"/Principal.Controls;component/Themes/V2/{item}_Win11.xaml", UriKind.RelativeOrAbsolute);
                    }
                    else
                    {
                        resourceDictionary.Source = new Uri($"/Principal.Controls;component/Themes/V2/{item}.xaml", UriKind.RelativeOrAbsolute);
                    }
                    InitResourcesForOffice2013Theme(resourceDictionary);
                    dicts[(int)item] = resourceDictionary;
                    currentResourceDictionary.MergedDictionaries.Add(resourceDictionary);
                }
            }
            currentResourceDictionary.EndInit();
            Resources.Clean();
        }
    }
}
