using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Principal.Server.Objects;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiProcessorStatus
{
	Paused = 1,
	Started,
	Stopped,
	NotInitialized
}
