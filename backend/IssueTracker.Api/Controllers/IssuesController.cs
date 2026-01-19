using IssueTracker.Api.Data;
using IssueTracker.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Api.Controllers;

[ApiController]
[Route("api/issues")]
public class IssuesController : ControllerBase
{
    private readonly AppDbContext _db;
    public IssuesController(AppDbContext db) => _db = db;

    [HttpPatch("{issueId:guid}/status")] //Actualiza el estado de un issue por id de issue
    public async Task<ActionResult<IssueDto>> UpdateStatus(Guid issueId, [FromBody] UpdateIssueStatusRequest req, CancellationToken ct)
    {
        var issue = await _db.Issues.FirstOrDefaultAsync(i => i.Id == issueId, ct);
        if (issue is null) return NotFound();

        issue.Status = req.Status;
        issue.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return Ok(new IssueDto(issue.Id, issue.ProjectId, issue.Title, issue.Description, issue.Status, issue.CreatedAt, issue.UpdatedAt));
    }
}
