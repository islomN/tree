using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

[Table("journals")]
public record Journal(
    [property: Column("id")]
    int Id,
    [property: Column("event_id")]
    long EventId,
    [property: Column("text")]
    string Text)
{
    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}