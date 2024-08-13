using Database.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Database;

public sealed class EntityContext: BaseEntityContext
{
    public EntityContext(IOptions<EntityContextOptions> options, bool isUseLazyLoading)
    {
        ConnectionString = options.Value.ConnectionString;
        IsUseLazyLoading = isUseLazyLoading;
    }
    
    public EntityContext(IOptions<EntityContextOptions> options)
    {
        ConnectionString = options.Value.ConnectionString;
    }
    
    public EntityContext(string connectionString)
    {
        ConnectionString = connectionString;
    }
    
    public DbSet<Tree> Trees { get; set; }
    
    public DbSet<Journal> Journals { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tree>()
            .HasIndex(i => new { i.Name, i.FirstParentId, i.ParentId })
            .IsUnique();
        
        modelBuilder.Entity<Tree>()
            .HasOne(t => t.FirstParent)
            .WithMany()
            .HasForeignKey(t => t.FirstParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies(IsUseLazyLoading)
            .UseNpgsql(
                ConnectionString,
                builder =>
                {
                    builder.EnableRetryOnFailure(
                        5,
                        TimeSpan.FromSeconds(2), 
                        null);
                });
    }
}