using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Resumi.Infra.Constants;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra;

/// <summary>
/// Utilitário para configuração e inicialização de serviços da aplicação conforme contexto.
/// </summary>
public static class Startup
{
    /// <summary>
    /// Configure suporte para autenticação JWT.
    /// </summary>
    /// <param name="builder">
    /// Instância do construtor da aplicação web.
    /// </param>
    /// <exception cref="InfrastructureException" />
    public static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        var jwtIssuer = builder.Configuration[EnvironmentVariables.JwtIssuer]
                        ?? throw new InfrastructureException(
                            $"Environment variable '{EnvironmentVariables.JwtIssuer}' is not set.");

        var jwtAudience = builder.Configuration[EnvironmentVariables.JwtAudience]
                          ?? throw new InfrastructureException(
                              $"Environment variable '{EnvironmentVariables.JwtAudience}' is not set.");

        var jwtSigningKey = builder.Configuration[EnvironmentVariables.JwtSigningKey]
                            ?? throw new InfrastructureException(
                                $"Environment variable '{EnvironmentVariables.JwtSigningKey}' is not set.");

        builder.Services.AddAuthentication()
            .AddJwtBearer("jwt-cookie",
                options =>
                {
                    options.Authority = jwtIssuer;
                    options.Audience = jwtAudience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey)),
                        ValidateLifetime = true
                    };
                    options.MapInboundClaims = false;
                    options.Events = new JwtBearerEvents

                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Cookies[AuthConstants.JwtCookie];

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
    }
}