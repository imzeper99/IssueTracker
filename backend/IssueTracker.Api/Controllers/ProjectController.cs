using IssueTracker.Api.Data;
using IssueTracker.Api.Dtos;
using IssueTracker.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProjectsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects(CancellationToken ct)
    {
        var projects = await _db.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto(p.Id, p.Name, p.CreatedAt))
            .ToListAsync(ct);

        return Ok(projects);
    }

    [HttpGet("{id:guid}")] //Devuelve proyecto por id
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id, CancellationToken ct)
    {
        var p = await _db.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (p is null) return NotFound();

        return Ok(new ProjectDto(p.Id, p.Name, p.CreatedAt));
    }

    [HttpPost] //Crea un nuevo proyecto
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest req, CancellationToken ct)
    {
        var entity = new ProjectEntity { Name = req.Name.Trim() };

        _db.Projects.Add(entity);
        await _db.SaveChangesAsync(ct);

        var dto = new ProjectDto(entity.Id, entity.Name, entity.CreatedAt);
        return CreatedAtAction(nameof(GetProject), new { id = entity.Id }, dto);
    }

    [HttpDelete("{id:guid}")] //Elimina un proyecto por id
    public async Task<IActionResult> DeleteProject(Guid id, CancellationToken ct)
    {
        var entity = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();

        _db.Projects.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpGet("{projectId:guid}/issues")] //Devuelve issues de un proyecto por id de proyecto
    public async Task<ActionResult<List<IssueDto>>> GetIssues(Guid projectId, CancellationToken ct)
    {
        var exists = await _db.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId, ct);
        if (!exists) return NotFound("Project not found.");

        var issues = await _db.Issues
            .AsNoTracking()
            .Where(i => i.ProjectId == projectId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new IssueDto(i.Id, i.ProjectId, i.Title, i.Description, i.Status, i.CreatedAt, i.UpdatedAt))
            .ToListAsync(ct);

        return Ok(issues);
    }

    [HttpPost("{projectId:guid}/issues")] //Crea un nuevo issue en un proyecto por id de proyecto
    public async Task<ActionResult<IssueDto>> CreateIssue(Guid projectId, [FromBody] CreateIssueRequest req, CancellationToken ct)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == projectId, ct);
        if (project is null) return NotFound("Project not found.");

        var issue = new IssueEntity
        {
            ProjectId = projectId,
            Title = req.Title.Trim(),
            Description = req.Description?.Trim()
        };

        _db.Issues.Add(issue);
        await _db.SaveChangesAsync(ct);

        return Ok(new IssueDto(issue.Id, issue.ProjectId, issue.Title, issue.Description, issue.Status, issue.CreatedAt, issue.UpdatedAt));
    }
}
