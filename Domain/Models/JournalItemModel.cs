using Newtonsoft.Json;

namespace Domain.Models;

public record JournalItemModel(
    [property: JsonProperty("id")]
    int Id,
    [property: JsonProperty("eventId")]
    long EventId,
    [property: JsonProperty("createdAt")]
    DateTime CreatedAt);