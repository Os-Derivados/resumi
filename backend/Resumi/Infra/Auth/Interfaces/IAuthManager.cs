using Resumi.App.Data.Models;

namespace Resumi.Infra.Auth.Interfaces;

/// <summary>
/// Este contrato define APIs para controle de autenticação e autorização.
/// </summary>
public interface IAuthManager
{
    /// <summary>
    /// Gera uma credencial JWT para o usuário autenticado.
    /// </summary>
    /// <param name="user">O usuário autenticado.</param>
    /// <returns>
    /// Uma <see cref="Task" /> representando uma instância de <see cref="AuthResponse" />.
    /// </returns>
    public AuthResponse NewAuthResponse(AppUser user);
}