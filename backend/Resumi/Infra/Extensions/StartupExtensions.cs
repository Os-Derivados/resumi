using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resumi.App.Services;
using Resumi.App.Validators;
using Resumi.Domain.Validators;
using Resumi.Infra.AuthZ;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Inclui os serviços de domínio no contêiner de injeção de dependência.
    /// Esses serviços são responsáveis pela lógica de negócios da aplicação.
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde os serviços de domínio serão registrados.
    /// </param>
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ResumeService>();
        services.AddScoped<UserService>();
        services.AddScoped<CertificateService>();
        services.AddScoped<DegreeService>();
        services.AddScoped<ExperienceService>();
        services.AddScoped<VolunteershipService>();
    }

    /// <summary>
    /// Inclui os validadores de domínio no contêiner de injeção de dependência.
    /// Esses validadores são responsáveis por garantir que os dados atendam às regras de negócio antes
    /// de serem processados ou persistidos.
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde os validadores de domínio serão registrados.
    /// </param>
    public static void AddDomainValidators(this IServiceCollection services)
    {
        services.AddScoped<ResumeValidator>();
        services.AddScoped<DegreeValidator>();
        services.AddScoped<CertificateValidator>();
        services.AddScoped<ExperienceValidator>();
        services.AddScoped<UserValidator>();
        services.AddScoped<VolunteershipValidator>();
    }

    public static void AddQueryValidators(this IServiceCollection services)
    {
        services.AddScoped<UserQueryValidator>();
        services.AddScoped<ResumeQueryValidator>();
    }

    /// <summary>
    /// Configura a documentação da API usando Swagger.
    /// Isso permite que a API gere uma documentação conforme a especificação OpenAPI.
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde a configuração do Swagger será registrada.
    /// </param>
    public static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Resumi API",
                    Version = "v1",
                    Description = "API REST para gerenciamento de currículos.",
                }
            );
        });
    }

    /// <summary>
    /// Inclui os mapeadores de entidades para DTOs no contêiner de injeção de dependência.
    /// </summary>
    /// <param name="services">O contêiner de serviços onde os mapeadores serão registrados.</param>
    public static void AddApiMappers(this IServiceCollection services)
    {
        services.AddScoped<UserMapper>();
        services.AddScoped<ExperienceMapper>();
        services.AddScoped<DegreeMapper>();
        services.AddScoped<VolunteershipMapper>();
        services.AddScoped<CertificateMapper>();
    }

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
                    options.RequireHttpsMetadata = false;
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

    /// <summary>
    /// Injeta as configurações de credenciais para autenticação JWT no contêiner de serviços.
    /// </summary>
    /// <param name="builder">
    /// Instância do construtor da aplicação web.
    /// </param>
    /// <exception cref="InfrastructureException" />
    public static void AddJwtSettings(this WebApplicationBuilder builder)
    {
        JwtAuthSettings jwtSettings = new()
        {
            Issuer = builder.Configuration[EnvironmentVariables.JwtIssuer]
                     ?? throw new InfrastructureException(
                         $"Environment variable '{EnvironmentVariables.JwtIssuer}' is not set."),
            Audience = builder.Configuration[EnvironmentVariables.JwtAudience]
                       ?? throw new InfrastructureException(
                           $"Environment variable '{EnvironmentVariables.JwtAudience}' is not set."),
            Secret = builder.Configuration[EnvironmentVariables.JwtSigningKey]
                     ?? throw new InfrastructureException(
                         $"Environment variable '{EnvironmentVariables.JwtSigningKey}' is not set.")
        };

        builder.Services.AddSingleton(jwtSettings);
    }

    /// <summary>
    /// Habilita a espera pela conexão de um depurador, se a variável de ambiente "WAIT_FOR_DEBUGGER" estiver definida como "true".
    /// </summary>
    /// <exception cref="InfrastructureException">Se a configuração de depurador foi habilitada e o programa não conseguiu conectar ao depurador dentro do tempo limite.</exception>
    public static async Task WaitForDebugger(bool wait = false)
    {
        if (!wait) return;

        var timeoutSeconds = int.TryParse(
            Environment.GetEnvironmentVariable("WAIT_FOR_DEBUGGER_TIMEOUT_SECONDS"),
            out var configuredTimeoutSeconds)
            ? configuredTimeoutSeconds
            : 60;

        var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

        while (!Debugger.IsAttached && DateTime.UtcNow < deadline)
        {
            await Task.Delay(200);
        }

        if (!Debugger.IsAttached)
        {
            throw new InfrastructureException(
                $"Waited for {timeoutSeconds} seconds, but the debugger was not attached.");
        }
    }
}