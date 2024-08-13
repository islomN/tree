using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain.Models;

public record RenameTreeRequest(
    [property: JsonProperty("treeName")]
    [Required]
    string TreeName, 
    [Required]
    [property: JsonProperty("nodeId")]
    int NodeId,
    [Required]
    [property: JsonProperty("newNodeName")]
    string NewNodeName);