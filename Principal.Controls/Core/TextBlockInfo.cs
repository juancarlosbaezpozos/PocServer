using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class TextBlockInfo : Control, IStiWpfV2Control
    {
        private Image imageInfo;

        private string toolTipBody;

        private string toolTipHeader;

        private UIElement toolTipBodyObject;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBlockInfo), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public TextBlockInfo()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                StiWpfV2ThemeHelper.LoadControlTheme(this);
                base.IsTabStop = false;
            }
        }

        public StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.TextBlockInfo;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            imageInfo = GetTemplateChild("imageInfo") as Image;
            UpdateToolTip();
        }

        public void SetToolTipText(string body)
        {
            toolTipHeader = null;
            toolTipBody = body;
            UpdateToolTip();
        }

        public void SetToolTipText(string header, string body)
        {
            toolTipHeader = header;
            toolTipBody = body;
            UpdateToolTip();
        }

        public void SetToolTipText(UIElement body)
        {
            toolTipBodyObject = body;
            UpdateToolTip();
        }

        private void UpdateToolTip()
        {
            if (imageInfo != null && (toolTipBody != null || toolTipBodyObject != null))
            {
                if (imageInfo.Source == null)
                {
                    StiReportWpfImages.InitImage(imageInfo, StiReportWpfImages.Editor.Information());
                }
                if (toolTipBodyObject != null)
                {
                    imageInfo.ToolTip = toolTipBodyObject;
                }
                else
                {
                    imageInfo.ToolTip = StiToolTipHelper.Get(toolTipHeader, toolTipBody, 300);
                }
            }
        }
    }
}
