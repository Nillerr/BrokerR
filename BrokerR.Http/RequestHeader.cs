using System.Text.Json.Serialization;

namespace BrokerR.Http
{
    public sealed class RequestHeader
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("value")]
        public string Value { get; set; } = null!;
    }
}