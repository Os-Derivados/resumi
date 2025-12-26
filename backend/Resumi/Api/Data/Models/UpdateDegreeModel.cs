using Resumi.Api.Data.Models;

public record UpdateDegreeModel : UpdateResumeNodeModel
{
    public string? Highlights { get; init; }
    public string? DegreeLevel { get; init; }
}
