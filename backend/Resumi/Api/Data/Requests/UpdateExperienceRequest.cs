using Resumi.Domain.Models;

namespace Resumi.Api.Data.Requests;

/// <summary>
/// Representa um objeto de parâmetros para a atualização
/// de um <see cref="Experience"/> dentro de um <see cref="Resume"/>.
/// </summary>
public record UpdateExperienceRequest(string? Highlights) : UpdateResumeNodeRequest;
