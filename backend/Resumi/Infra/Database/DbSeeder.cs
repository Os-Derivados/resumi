using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Resumi.Api.Data.Models;
using Resumi.Domain.Models;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Database;

public static class DbSeeder
{
    public static async Task<bool> SeedDatabaseAsync(IServiceProvider provider)
    {
        var dbContext = provider.GetRequiredService<AppDbContext>()
                        ?? throw new InfrastructureException("AppDbContext service is not available.");

        var userManager = provider.GetRequiredService<UserManager<AppUser>>()
                          ?? throw new InfrastructureException("UserManager service is not available.");

        var userMapper = provider.GetRequiredService<UserMapper>()
                         ?? throw new InfrastructureException("UserMapper service is not available.");

        var adminUserJson = Environment.GetEnvironmentVariable(EnvironmentVariables.AdminUser);
        var adminUserModel = JsonSerializer.Deserialize<CreateUserModel>(adminUserJson!)
                             ?? throw new InfrastructureException(
                                 "Failed to deserialize admin user data from environment variable.");

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var existingAdmin = await userManager.FindByEmailAsync(adminUserModel.Email);

            if (existingAdmin is not null) return false;

            var newAdminUser = userMapper.NewDomainModel(adminUserModel) ??
                               throw new InfrastructureException("Failed to map admin user model to domain model.");
            var createResult = await userManager.CreateAsync(newAdminUser, adminUserModel.Password);

            if (!createResult.Succeeded) return false;

            var addToRoleResult = await userManager.AddToRoleAsync(newAdminUser, AuthConstants.AdminRole);

            if (!addToRoleResult.Succeeded) return false;

            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();

            throw;
        }
    }
}