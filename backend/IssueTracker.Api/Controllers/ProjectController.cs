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
    //EndPoint para probar DB
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> Get(CancellationToken ct) 
    {
        var list = await _db.Projects
            .AsNoTracking() //Mejora el rendimiento al no rastrear las entidades
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectDto(p.Id, p.Name, p.CreatedAt))
            .ToListAsync(ct);

        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectRequest req, CancellationToken ct)
    {
        var entity = new ProjectEntity { Name = req.Name.Trim() };
        _db.Projects.Add(entity);
        await _db.SaveChangesAsync(ct);

        return Ok(new ProjectDto(entity.Id, entity.Name, entity.CreatedAt));
    }
}
