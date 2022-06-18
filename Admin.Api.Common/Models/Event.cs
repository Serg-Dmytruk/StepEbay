using System.Text.Json.Serialization;

namespace StepEbay.Admin.Api.Common.Models
{
    public class Event
    {
        [JsonPropertyName("Timestamp")] public DateTimeOffset Timestamp { get; set; }

        [JsonPropertyName("Level")] public string Level { get; set; }

        [JsonPropertyName("MessageTemplate")] public string MessageTemplate { get; set; }

        [JsonPropertyName("RenderedMessage")] public string RenderedMessage { get; set; }

        [JsonPropertyName("Exception")] public string Exception { get; set; }

        [JsonPropertyName("Properties")] public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("CircuitId")] public string CircuitId { get; set; }

        [JsonPropertyName("EventId")] public EventId EventId { get; set; }

        [JsonPropertyName("SourceContext")] public string SourceContext { get; set; }

        [JsonPropertyName("TransportConnectionId")]
        public string TransportConnectionId { get; set; }

        [JsonPropertyName("RequestId")] public string RequestId { get; set; }

        [JsonPropertyName("RequestPath")] public string RequestPath { get; set; }

        [JsonPropertyName("ConnectionId")] public string ConnectionId { get; set; }
    }

    public class EventId
    {
        [JsonPropertyName("Id")] public long Id { get; set; }

        [JsonPropertyName("Name")] public string Name { get; set; }
    }
}
