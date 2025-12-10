using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a atualização
/// de um <see cref="Volunteership"/> dentro de um <see cref="Resume"/>.
/// </summary>
public record UpdateVolunteershipModel : UpdateResumeNodeModel;
