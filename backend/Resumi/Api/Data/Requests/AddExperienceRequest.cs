namespace Resumi.Api.Data.Requests;

/// <summary>
/// Representa um objeto de parâmetros para a atribuição
/// de um <see cref="Experience"/> dentro de um <see cref="Resume"/>.
/// </summary>
public record AddExperienceRequest(string? Highlights) : AddResumeNodeRequest;
