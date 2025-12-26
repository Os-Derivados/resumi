using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Objeto de parâmetros para a leitura de um usuário <see cref="AppUser"/>.
/// </summary>
public record UserModel
{
    public int Id { get; init; }
    public string? FullName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
}