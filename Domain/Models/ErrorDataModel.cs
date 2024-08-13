using Newtonsoft.Json;

namespace Domain.Models;

public record ErrorDataModel(
    [property: JsonProperty("message")]
    string Message);