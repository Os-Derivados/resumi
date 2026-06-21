using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Requests;

/// <summary>
/// Objeto de parâmetros para autenticação de usuários.
/// </summary>
public record LoginRequest
{
    [Required] public required string Email { get; init; }

    [Required] public required string Password { get; init; }
}