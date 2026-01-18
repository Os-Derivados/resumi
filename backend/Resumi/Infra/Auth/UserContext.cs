using System.Security.Claims;

namespace Resumi.Infra.Auth;


/// <summary>
/// Oferece métodos para extrair informações do contexto do usuário autenticado.
/// </summary>
public class UserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User
        ?? throw new UnauthorizedAccessException("No HTTP context or user found.");

    public int GetUserId()
    {
        var userId = User.FindFirst("nameid")
            ?? throw new UnauthorizedAccessException("User ID claim not found.");
        
        return int.Parse(userId.Value);
    }
}