using System.ComponentModel;

namespace Principal.Controls.Core
{
    [DesignTimeVisible(false)]
    public class CloudGroupBox : StiGroupBox
    {
        public override StiWpfV2ControlID GetV2ID()
        {
            return StiWpfV2ControlID.CloudGroupBox;
        }
    }
}
