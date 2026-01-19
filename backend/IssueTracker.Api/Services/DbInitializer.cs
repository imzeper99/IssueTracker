using IssueTracker.Api.Data;
using IssueTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Services;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Aplica migraciones automáticamente al arrancar.
        await db.Database.MigrateAsync();

        if (await db.Projects.AnyAsync())
            return;

        var project = new ProjectEntity { Name = "Demo Project" };
        db.Projects.Add(project);

        // Agrega algunos issues de ejemplo.
        db.Issues.AddRange(
            new IssueEntity
            {
                Project = project,
                Title = "Set up backend API",
                Description = "Projects + Issues endpoints + SQLite",
                Status = IssueStatus.InProgress
            },
            new IssueEntity
            {
                Project = project,
                Title = "Create Angular UI",
                Description = "Projects list + Issues table + status update",
                Status = IssueStatus.Open
            }
        );

        await db.SaveChangesAsync();
    }
}
