using Microsoft.OpenApi.Models;
using Resumi.App.Services;
using Resumi.App.Services.Interfaces;
using Resumi.App.Services.Validators;

namespace Resumi.Infra.Extensions;

public static class StartupExtensions
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IResumeService, ResumeService>();
    }

    public static void AddDomainValidators(this IServiceCollection services)
    {
        services.AddScoped<IResumeValidator, ResumeValidator>();
    }

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