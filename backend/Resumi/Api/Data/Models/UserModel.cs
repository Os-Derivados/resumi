using System.Security.Claims;
using Resumi.App.Data.Models;
using Resumi.Infra.Auth.Constants;

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

    /// <summary>
    /// Cria uma instância de <see cref="UserModel"/> a partir de um <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">
    /// O <see cref="ClaimsPrincipal"/> contendo as claims do usuário autenticado.
    /// </param>
    /// <returns>
    /// Uma instância de <see cref="UserModel"/> ou <c>null</c> se o <paramref name="claimsPrincipal"/> for <c>null</c>.
    /// </returns>
    public static UserModel? FromClaimsPrincipal(ClaimsPrincipal? claimsPrincipal)
    {
        if (claimsPrincipal is null) return null;
        
        return new UserModel
        {
            Id = int.Parse(claimsPrincipal.FindFirst(SessionConstants.UserIdClaim)!.Value),
            FullName = claimsPrincipal.FindFirst(SessionConstants.UserNameClaim)!.Value,
            Email = claimsPrincipal.FindFirst(SessionConstants.EmailClaim)!.Value,
            PhoneNumber = claimsPrincipal.FindFirst(ClaimTypes.MobilePhone)!.Value
        };
    }
}