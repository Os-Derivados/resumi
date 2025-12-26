namespace Resumi.Api.Data.Models;

public record DegreeModel : ResumeNodeModel
{
    public string? Highlights { get; init; }
    public string? Level { get; init; }
}