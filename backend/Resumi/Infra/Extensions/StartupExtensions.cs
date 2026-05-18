using Microsoft.OpenApi.Models;
using Resumi.App.Services;
using Resumi.App.Validators;
using Resumi.Domain.Validators;

namespace Resumi.Infra.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Inclui os módulos de domínio no contêiner de injeção de dependência.
    /// Esses módulos são responsáveis por agrupar funcionalidades relacionadas a um determinado domínio da aplicação.
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde os módulos de domínio serão registrados.
    /// </param>
    public static void AddDomainModules(this IServiceCollection services)
    {
        services.AddScoped<ResumesModule>();
        services.AddScoped<UsersModule>();
        services.AddScoped<DegreesModule>();
        services.AddScoped<CertificatesModule>();
        services.AddScoped<ExperiencesModule>();
        services.AddScoped<VolunteershipsModule>();
    }

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
    }

    /// <summary>
    /// Registra os mapeadores de entidades no contêiner de injeção de dependência.
    /// Esses mapeadores são responsáveis por converter entre modelos de domínio e modelos de dados.
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde os mapeadores serão registrados.
    /// </param>
    public static void AddEntityMappers(this IServiceCollection services)
    {
        services.AddScoped<IResumeMapper, ResumeMapper>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ICertificateMapper, CertificateMapper>();
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
}