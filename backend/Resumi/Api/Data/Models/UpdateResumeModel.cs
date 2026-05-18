using System.ComponentModel.DataAnnotations;
using Resumi.Domain.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para atualizar um <see cref="Resume"/> existente.
/// </summary>
/// <param name="UserId">O identificador do usuário proprietário do currículo.</param>
/// <param name="Title">O possível novo título do currículo.</param>
public record UpdateResumeModel(
    [Required] int UserId,
    [MaxLength(128)] string? Title,
    [MaxLength(128)] string? OwnerName,
    string? Location,
    [EmailAddress] string? Email,
    [Phone] string? PhoneNumber);
