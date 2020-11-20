using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BrokerR.Http
{
    public sealed class WebhookRequest
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = null!;
        
        [JsonPropertyName("headers")]
        public List<RequestHeader> Headers { get; set; } = new List<RequestHeader>();
        
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}