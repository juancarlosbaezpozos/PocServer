using System.Collections.Generic;

namespace Principal.Server.Objects;

public interface IStiCore
{
    string SessionKey { get; set; }

    List<IStiProcessor> Processors { get; set; }
}
