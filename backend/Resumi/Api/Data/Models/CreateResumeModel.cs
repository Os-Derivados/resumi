using System.ComponentModel.DataAnnotations;
using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para criar um novo <see cref="Resume"/>.
/// </summary>
/// <param name="Title">O título do currículo.</param>
/// <param name="UserId">O identificador do usuário proprietário do currículo.</param>
public record CreateResumeModel([Required] string Title, [Required] int UserId);
