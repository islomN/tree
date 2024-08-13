namespace Domain.Models;

public record TreeModel(
    int Id,
    string Name,
    int? ParentId);