using Resumi.Api.Data.Models;

public record UpdateDegreeModel : UpdateResumeNodeModel
{
	public string? Highlights { get; init; }
	public string? Level { get; init; }
}
