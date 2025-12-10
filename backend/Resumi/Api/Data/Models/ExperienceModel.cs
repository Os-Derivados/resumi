namespace Resumi.Api.Data.Models;

public record ExperienceModel : ResumeNodeModel
{
    public string? Highlights { get; init; }
}