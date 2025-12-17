using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Database;

public static class DbSeeder
{
    public static async Task<bool> SeedDatabaseAsync(IServiceProvider provider)
    {
        var userManager = provider.GetRequiredService<UserManager<AppUser>>()
                          ?? throw new InfrastructureException("UserManager service is not available.");

        var userMapper = provider.GetRequiredService<IUserMapper>()
                         ?? throw new InfrastructureException("UserMapper service is not available.");

        var adminUserJson = Environment.GetEnvironmentVariable(EnvironmentVariables.AdminUser);
        var adminUserModel = JsonSerializer.Deserialize<CreateUserModel>(adminUserJson!)
                             ?? throw new InfrastructureException(
                                 "Failed to deserialize admin user data from environment variable.");

        var existingAdmin = await userManager.FindByEmailAsync(adminUserModel.Email);

        if (existingAdmin is not null) return false;

        var newAdminUser = userMapper.NewDomainModel(adminUserModel)
                           ?? throw new InfrastructureException("Failed to map admin user model to domain model.");
        var createResult = await userManager.CreateAsync(newAdminUser);

        return createResult.Succeeded;
    }
}