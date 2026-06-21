using System.ComponentModel.DataAnnotations;
using Resumi.Domain.Models;

namespace Resumi.Api.Data.Requests;

/// <summary>
/// Objeto de parâmetros para a criação de um novo usuário <see cref="AppUser"/>.
/// </summary>
public record CreateUserRequest
{
  [Required] public required string FullName { get; init; }

  [Required] [Phone] public required string PhoneNumber { get; init; }

  [Required] [EmailAddress] public required string Email { get; init; }

  [Required] public required string Password { get; init; }
};