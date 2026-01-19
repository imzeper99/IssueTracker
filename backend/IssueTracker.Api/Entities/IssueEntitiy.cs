namespace IssueTracker.Api.Entities;

public enum IssueStatus
{
    Open = 0,
    InProgress = 1,
    Done = 2
}

public class IssueEntity
{
    public Guid Id { get; set; } = Guid.NewGuid(); //Guid como identificador unico

    public Guid ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public IssueStatus Status { get; set; } = IssueStatus.Open;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
