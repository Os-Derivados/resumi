using System.Security.Claims;

namespace Resumi.Infra.Auth;


/// <summary>
/// Oferece métodos para extrair informações do contexto do usuário autenticado.
/// </summary>
public class UserContext
{
    public readonly ClaimsPrincipal User;

    public UserContext(ClaimsPrincipal user)
    {
        User = user;
    }

    public int GetUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim not found.");
        
        return int.Parse(userId.Value);
    }
}