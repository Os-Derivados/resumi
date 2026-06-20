using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Requests;

/// <summary>
/// Objeto de parâmetros para a criação de um novo usuário <see cref="AppUser"/>.
/// </summary>
public record CreateUserRequest
{
  [Required] public string FullName { get; init; }

  [Required] [Phone] public string PhoneNumber { get; init; }

  [Required] [EmailAddress] public string Email { get; init; }

  [Required] public string Password { get; init; }
};