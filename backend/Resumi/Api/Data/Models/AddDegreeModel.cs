using Resumi.Domain.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a atribuição
/// de um <see cref="Degree"/> dentro de um <see cref="Resume"/>.
/// </summary>
public record AddDegreeModel : AddResumeNodeModel
{
    public string? Highlights { get; init; }
    public string? Level { get; init; }
}
