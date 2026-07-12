using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.AuthZ.Interfaces;
using Resumi.Infra.Constants;
using Resumi.Infra.Database;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Exceptions;
using Resumi.Infra.Extensions;

if (bool.TryParse(Environment.GetEnvironmentVariable("WAIT_FOR_DEBUGGER"), out var wait))
{
	await StartupExtensions.WaitForDebugger(wait);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
	var allowedOrigin = Environment.GetEnvironmentVariable(EnvironmentVariables.AllowedOrigin)
	                    ?? throw new InfrastructureException(
		                    $"Environment variable '{EnvironmentVariables.AllowedOrigin}' is not set.");

	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins(allowedOrigin)
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

builder.Services.AddControllers();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(defaultConnection))
{
	builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(defaultConnection));
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiDocumentation();
builder.Services.AddDomainValidators();
builder.Services.AddQueryValidators();
builder.Services.AddDomainServices();
builder.Services.AddIdentityCore<AppUser>(AppUserManager.IdentityOptionsSetup)
	.AddRoles<AppRole>()
	.AddUserManager<AppUserManager>()
	.AddRoleManager<RoleManager<AppRole>>()
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddSingleton<AuditInterceptor>();
builder.Services.AddScoped<UserContext>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddExceptionHandler((options) =>
{
	options.ExceptionHandler = async context =>
	{
		var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
		var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

		logger.LogError(exception, "An unhandled exception occurred: {Message}", exception?.Message);

		context.Response.StatusCode = StatusCodes.Status500InternalServerError;

		await context.Response.WriteAsJsonAsync(new
		{
			Error = "An unexpected error occurred. Please try again later."
		});
	};
});

builder.Services.AddProblemDetails();
builder.AddJwtSettings();
builder.AddJwtAuth();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddApiMappers();

var app = builder.Build();

using var seedScope = app.Services.CreateScope();
await DbSeeder.SeedDatabaseAsync(seedScope.ServiceProvider);

app.UseExceptionHandler();
app.UseCors();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumi API v1"); });
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


