using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Api.Dtos;

public record ProjectDto(Guid Id, string Name, DateTime CreatedAt);

public record CreateProjectRequest(
    [Required, MinLength(2), MaxLength(120)] string Name
);
