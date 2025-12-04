using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Objeto de parâmetros para a leitura de um usuário <see cref="AppUser"/>.
/// </summary>
public record ReadUserModel(string FullName, string PhoneNumber, string Email);