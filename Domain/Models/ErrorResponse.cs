using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain.Models;

public record ErrorResponse(
    [property: JsonProperty("type")]
    string Type,
    [property: JsonProperty("id")]
    long Id,
    [property: JsonProperty("data")]
    ErrorDataModel Data);