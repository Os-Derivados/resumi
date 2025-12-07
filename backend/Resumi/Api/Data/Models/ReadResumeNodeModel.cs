namespace Resumi.Api.Data.Models;

public record ReadResumeNodeModel
{
    public int Id { get; init; }
    public int ResumeId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? InstitutionName { get; init; }
    public string? Location { get; init; }
    public bool IsRemote { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public bool StillEngaged { get; init; }
}