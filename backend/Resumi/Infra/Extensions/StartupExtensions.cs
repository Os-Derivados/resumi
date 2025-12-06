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
}