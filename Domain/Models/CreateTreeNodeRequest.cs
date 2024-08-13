using Newtonsoft.Json;

namespace Domain.Models;

public record CreateTreeNodeRequest(
    [property: JsonProperty("treeName")]
    string TreeName,
    [property: JsonProperty("arentNodeId")]
    int ParentNodeId,
    [property: JsonProperty("nodeName")]
    string NodeName);