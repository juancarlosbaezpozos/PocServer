namespace Principal.Server.Objects;

public interface IStiProcessor
{
    string ProcessorName { get; }

    StiProcessorStatus ProcessorStatus { get; set; }

    void Start();

    void Pause();

    void Continue();

    void Stop();

    void Break();

    StiProcessorInfo GetProcessorInfo();
}
