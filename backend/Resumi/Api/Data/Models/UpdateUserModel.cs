using System.ComponentModel.DataAnnotations;
using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Objeto de parâmetros para a atualização de um usuário <see cref="AppUser"/>.
/// </summary>
public record UpdateUserModel
{
  public string? FullName { get; init; }

  [Phone] public string? PhoneNumber { get; init; }

  [EmailAddress] public string? Email { get; init; }
}