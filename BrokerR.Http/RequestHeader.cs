using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace BrokerR.Http
{
    [PublicAPI]
    public sealed class RequestHeader
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("value")]
        public string Value { get; set; } = null!;
    }
}