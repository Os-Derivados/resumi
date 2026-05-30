namespace Resumi.Infra.AuthZ;

/// <summary>
/// Objeto de parâmetros de resposta para autenticação.
/// </summary>
public record AuthResponse
{
	public string? Token { get; init; }
	public DateTime ExpiresAt { get; init; }
}