using Newtonsoft.Json;

namespace Domain.Models;

public record Tree(
    [property: JsonProperty("id")]
    int Id,
    [property: JsonProperty("name")]
    string Name,
    [property: JsonProperty("children")]
    IReadOnlyCollection<Tree> Children);