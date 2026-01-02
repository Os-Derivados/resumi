namespace Resumi.Infra.Auth;

/// <summary>
/// Objeto de parâmetros de resposta para autenticação.
/// </summary>
public record AuthResponse
{
    public string? Token { get; init; }
    public DateTime ExpiresAt { get; init; }
}