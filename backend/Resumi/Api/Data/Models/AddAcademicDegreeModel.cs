using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a atribuição
/// de um <see cref="AcademicDegree"/> dentro de um <see cref="Resume"/>.
/// </summary>
public record AddAcademicDegreeModel(string? Highlights, string? DegreeLevel) : AddNodeModel;
