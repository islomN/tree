using Newtonsoft.Json;

namespace Domain.Models;

public record JournalListResponse(
    [property: JsonProperty("id")]
    int Skip,
    [property: JsonProperty("id")]
    int Count,
    [property: JsonProperty("id")]
    IReadOnlyCollection<JournalItemModel> Items);