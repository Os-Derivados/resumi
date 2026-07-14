using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Resumi.Api.Data.Requests;
using Resumi.App.Managers;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Database;

public static class DbSeeder
{
	public static async Task SeedDatabaseAsync(IServiceProvider provider)
	{
		var roleManager = provider.GetRequiredService<RoleManager<AppRole>>();
		var userManager = provider.GetRequiredService<AppUserManager>();
		var userMapper = provider.GetRequiredService<UserMapper>();
		var userService = provider.GetRequiredService<UserManager>();

		await EnsureRoleExistsAsync(roleManager, AuthConstants.AdminRole);

		var adminUserModel = GetAdminUserModel();
		var adminUser = await GetOrCreateAdminUserAsync(userManager, userService, userMapper, adminUserModel);
		await EnsureUserIsInRoleAsync(userManager, adminUser, AuthConstants.AdminRole);
	}

	private static async Task EnsureRoleExistsAsync(RoleManager<AppRole> roleManager, string roleName)
	{
		if (await roleManager.RoleExistsAsync(roleName))
		{
			return;
		}

		var createRoleResult = await roleManager.CreateAsync(new AppRole(roleName));
		if (!createRoleResult.Succeeded)
		{
			throw new InfrastructureException(
				$"Failed to create admin role: {string.Join(", ", createRoleResult.Errors.Select(e => e.Description))}");
		}
	}

	private static CreateUserRequest GetAdminUserModel()
	{
		var adminUserJson = Environment.GetEnvironmentVariable(EnvironmentVariables.AdminUser);

		return JsonSerializer.Deserialize<CreateUserRequest>(adminUserJson!)
		       ?? throw new InfrastructureException("Failed to deserialize admin user data from environment variable.");
	}

	private static async Task<AppUser> GetOrCreateAdminUserAsync(
		AppUserManager userManager,
		UserManager userService,
		UserMapper userMapper,
		CreateUserRequest adminUserModel)
	{
		var existingAdmin = await userManager.FindByEmailAsync(adminUserModel.Email);

		if (existingAdmin is not null) return existingAdmin;

		var newUser = userMapper.NewDomainModel(adminUserModel);
		var createUserResult = await userService.CreateAsync(newUser, adminUserModel.Password, CancellationToken.None);

		if (!createUserResult.Succeeded)
		{
			throw new InfrastructureException(
				$"Failed to create admin user: {string.Join(", ", createUserResult.AllErrors!)}");
		}

		return await userManager.FindByEmailAsync(adminUserModel.Email)
		       ?? throw new InfrastructureException("Admin should exist at this point.");
	}

	private static async Task EnsureUserIsInRoleAsync(AppUserManager userManager, AppUser user, string roleName)
	{
		if (await userManager.IsInRoleAsync(user, roleName)) return;

		var addToRoleResult = await userManager.AddToRoleAsync(user, roleName);

		if (!addToRoleResult.Succeeded)
		{
			throw new InfrastructureException(
				$"Failed to assign admin role to user: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
		}
	}
}
