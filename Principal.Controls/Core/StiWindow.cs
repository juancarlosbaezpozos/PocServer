using System;
using System.ComponentModel;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public abstract class StiWindow : Window, IStiWpfV2Control
    {
        protected string HelpUrl;

        public TaskCompletionSource<string> Completion;

        private const uint WS_EX_CONTEXTHELP = 1024u;

        private const uint WS_MINIMIZEBOX = 131072u;

        private const uint WS_MAXIMIZEBOX = 65536u;

        private const int GWL_STYLE = -16;

        private const int GWL_EXSTYLE = -20;

        private const int SWP_NOSIZE = 1;

        private const int SWP_NOMOVE = 2;

        private const int SWP_NOZORDER = 4;

        private const int SWP_FRAMECHANGED = 32;

        private const int WM_SYSCOMMAND = 274;

        private const int SC_CONTEXTHELP = 61824;

        public bool ShowIcon { get; set; } = true;


        public StiWindow()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                base.UseLayoutRounding = true;
                StiWpfV2ThemeHelper.InitTheme(this);
            }
        }

        public virtual StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.StiWindow;
        }

        [SecuritySafeCritical]
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (HelpUrl != null)
            {
                IntPtr handle = new WindowInteropHelper(this).Handle;
                uint windowLong = RibbonWinApi.GetWindowLong(handle, -16);
                windowLong &= 0xFFFCFFFFu;
                RibbonWinApi.SetWindowLong(handle, -16, windowLong);
                windowLong = RibbonWinApi.GetWindowLong(handle, -20);
                windowLong |= 0x400u;
                RibbonWinApi.SetWindowLong(handle, -20, windowLong);
                RibbonWinApi.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
                ((HwndSource)PresentationSource.FromVisual(this))?.AddHook(HelpHook);
            }
            IconHelper.RemoveIcon(this);
        }

        private IntPtr HelpHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 274 && ((int)wParam & 0xFFF0) == 61824)
            {
                StiWpfHelpProvider.ShowHelpViewer(HelpUrl);
                handled = true;
            }
            return IntPtr.Zero;
        }
    }
}
