using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables;

[Table("trees")]
public record Tree(
    [property: Column("id")]
    int Id,
    [property: Column("name")]
    [property: Required]
    [property: MaxLength(150)]
    string Name,
    [property: Column("first_parent_id")]
    int? FirstParentId,
    [property: Column("parent_id")]
    int? ParentId)
{
    [ForeignKey(nameof(FirstParentId))]
    public virtual Tree? FirstParent { get; init; }
    
    [ForeignKey(nameof(ParentId))]
    public virtual Tree? Parent { get; init; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    
    public virtual ICollection<Tree> Children { get; init; }
}