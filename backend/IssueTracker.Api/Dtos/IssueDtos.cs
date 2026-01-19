using System.ComponentModel.DataAnnotations;
using IssueTracker.Api.Entities;

namespace IssueTracker.Api.Dtos;

//Usamos records para definir DTOs (Data Transfer Objects) que son inmutables y concisos. Simplificamos el trabajo con datos.

public record IssueDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    IssueStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateIssueRequest(
    [Required, MinLength(3), MaxLength(200)] string Title,
    [MaxLength(2000)] string? Description
);

public record UpdateIssueStatusRequest(
    [Required] IssueStatus Status
);
