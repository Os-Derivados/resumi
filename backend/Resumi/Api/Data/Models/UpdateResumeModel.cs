using System.ComponentModel.DataAnnotations;
using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para atualizar um <see cref="Resume"/> existente.
/// </summary>
/// <param name="Title">O possível novo título do currículo.</param>
/// <param name="UserId">O identificador do usuário proprietário do currículo.</param>
public record UpdateResumeModel(string? Title, [Required] int UserId);
