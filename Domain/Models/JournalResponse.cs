using Newtonsoft.Json;

namespace Domain.Models;

public record JournalResponse(
    [property: JsonProperty("id")]
    int Id,
    [property: JsonProperty("eventId")]
    long EventId,
    [property: JsonProperty("text")]
    string Text,
    [property: JsonProperty("createdAt")]
    DateTime CreatedAt);