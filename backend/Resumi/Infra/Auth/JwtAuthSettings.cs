namespace Resumi.Infra.Auth;

/// <summary>
/// Objeto de parâmetros que encapsula as configurações de autenticação.
/// </summary>
public record JwtAuthSettings
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}