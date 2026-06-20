namespace Resumi.Api.Data.Requests;

public record UpdateDegreeRequest : UpdateResumeNodeRequest
{
	public string? Highlights { get; init; }
	public string? Level { get; init; }
}