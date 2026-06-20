using System.ComponentModel.DataAnnotations;

namespace Resumi.Api.Data.Requests;

/// <summary>
/// Objeto de parâmetros para a atualização de um usuário <see cref="AppUser"/>.
/// </summary>
public record UpdateUserModel
{
  public string? FullName { get; init; }

  [Phone] public string? PhoneNumber { get; init; }

  [EmailAddress] public string? Email { get; init; }
}