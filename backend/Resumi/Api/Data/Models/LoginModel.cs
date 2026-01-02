using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Objeto de parâmetros para autenticação de usuários.
/// </summary>
public record LoginModel
{
    [Required] public required string Email { get; init; }

    [Required] public required string Password { get; init; }
}