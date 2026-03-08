namespace Resumi.Infra.Auth.Constants;

/// <summary>
/// Constantes relacionadas às sessões de usuário e respectivas claims.
/// </summary>
public static class SessionConstants
{
    public const string UserIdClaim = "nameid";
    public const string UserNameClaim = "unique_name";
    public const string EmailClaim = "email";
}