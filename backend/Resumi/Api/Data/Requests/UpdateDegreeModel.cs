using Resumi.Api.Data.Models;

namespace Resumi.Api.Data.Requests;

public record UpdateDegreeModel : UpdateResumeNodeModel
{
	public string? Highlights { get; init; }
	public string? Level { get; init; }
}