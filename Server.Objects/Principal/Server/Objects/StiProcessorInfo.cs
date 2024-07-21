using System.ComponentModel;

namespace Principal.Server.Objects;

public class StiProcessorInfo : StiKeyObject
{
    public string Name { get; set; }

    public string ProcessorName { get; set; }

    public string CurrentTask { get; set; }

    [DefaultValue(StiProcessorStatus.NotInitialized)]
    public StiProcessorStatus ProcessorStatus { get; set; } = StiProcessorStatus.NotInitialized;

    public override string ToString()
    {
        return ProcessorName;
    }

    public static bool Compare(StiProcessorInfo element1, StiProcessorInfo element2)
    {
        if (element1 == element2)
        {
            return true;
        }
        if (element1 != null && element2 != null && element1.Name == element2.Name && element1.ProcessorName == element2.ProcessorName && element1.CurrentTask == element2.CurrentTask)
        {
            return element1.ProcessorStatus == element2.ProcessorStatus;
        }
        return false;
    }
}
