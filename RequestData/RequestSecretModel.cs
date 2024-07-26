using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RequestData
{
    public partial class RequestSecretModel
    {
        [JsonProperty("SecretName", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretName { get; set; }
    }

    public partial class RequestSecretModel
    {
        public static RequestSecretModel FromJson(string json) => JsonConvert.DeserializeObject<RequestSecretModel>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this RequestSecretModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter{DateTimeStyles = System.Globalization.DateTimeStyles.AssumeUniversal}
            }
        };
    }
}
