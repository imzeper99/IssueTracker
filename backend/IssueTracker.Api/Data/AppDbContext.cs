using IssueTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Data;

//Definición de tablas y relaciones


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<IssueEntity> Issues => Set<IssueEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();

            e.HasMany(x => x.Issues)
             .WithOne(x => x.Project!)
             .HasForeignKey(x => x.ProjectId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<IssueEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(2000);
            e.Property(x => x.Status).IsRequired();
            e.Property(x => x.CreatedAt).IsRequired();
            e.Property(x => x.UpdatedAt);
        });
    }
}
