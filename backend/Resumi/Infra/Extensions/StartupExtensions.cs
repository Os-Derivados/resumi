using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Resumi.App.Data.Models;
using Resumi.App.Modules;
using Resumi.App.Services;
using Resumi.App.Services.Interfaces;
using Resumi.App.Services.Validators;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Database.Interfaces;
using Resumi.Infra.Database.Repositories;

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
        services.AddScoped<IDomainService<Resume>, ResumeService>();
        services.AddScoped<IDomainService<AppUser>, UserService>();
        services.AddScoped<IDomainService<Certificate>, CertificateService>();
        services.AddScoped<IDomainService<Degree>, DegreeService>();
        services.AddScoped<IDomainService<Experience>, ExperienceService>();
        services.AddScoped<IDomainService<Volunteership>, VolunteershipService>();
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
        services.AddScoped<IDomainValidator<Resume>, ResumeValidator>();
        services.AddScoped<IDomainValidator<Degree>, DegreeValidator>();
        services.AddScoped<IDomainValidator<Certificate>, CertificateValidator>();
        services.AddScoped<IDomainValidator<Experience>, ExperienceValidator>();
        services.AddScoped<IDomainValidator<AppUser>, UserValidator>();
        services.AddScoped<IDomainValidator<Volunteership>, VolunteershipValidator>();
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
    }


    /// <summary>
    /// Registra os repositórios de dados no contêiner de injeção de dependência.
    /// Esses repositórios são responsáveis por interagir com a camada de persistência de dados.
    /// <remarks>Para entidades <see cref="AppUser"/> e <see cref="IdentityRole{TKey}"/>,
    /// devem ser utilizados os componentes do Identity.</remarks>
    /// </summary>
    /// <param name="services">
    /// O contêiner de serviços onde os repositórios serão registrados.
    /// </param>
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Resume>, ResumeRepository>();
        services.AddScoped<IRepository<Experience>, ExperienceRepository>();
        services.AddScoped<IRepository<Degree>, DegreeRepository>();
        services.AddScoped<IRepository<Certificate>, CertificateRepository>();
        services.AddScoped<IRepository<Volunteership>, VolunteershipRepository>();
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