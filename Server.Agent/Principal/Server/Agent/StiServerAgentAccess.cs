using Principal.Server.Objects;

namespace Principal.Server.Agent;

public class StiServerAgentAccess : IStiServerAgentAccess
{
    public void Stop()
    {
        StiServerAgentHelper.StopService();
    }
}
