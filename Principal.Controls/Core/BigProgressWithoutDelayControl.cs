using System.ComponentModel;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public sealed class BigProgressWithoutDelayControl : BigProgressControl
    {
        public override StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.BigProgressWithoutDelayControl;
        }
    }
}
