using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resumi.App.Data.Models;
using Resumi.Infra;
using Resumi.Infra.Auth;
using Resumi.Infra.Auth.Interfaces;
using Resumi.Infra.Constants;
using Resumi.Infra.Database;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Exceptions;
using Resumi.Infra.Extensions;

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
            .AllowAnyMethod();
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
builder.Services.AddRepositories();
builder.Services.AddEntityMappers();
builder.Services.AddDomainValidators();
builder.Services.AddDomainServices();
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole<int>>()
    .AddRoleManager<RoleManager<IdentityRole<int>>>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddExceptionHandler((options) =>
{
    options.ExceptionHandler = async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        logger.LogError(exception, "An unhandled exception occurred.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new
        {
            Error = "An unexpected error occurred. Please try again later."
        });
    };
});

builder.Services.AddProblemDetails();
builder.Services.AddDomainModules();
builder.AddJwtSettings();
builder.AddJwtAuth();
builder.Services.AddScoped<IAuthManager, AuthManager>();

var app = builder.Build();

using var seedScope = app.Services.CreateScope();
_ = await DbSeeder.SeedDatabaseAsync(seedScope.ServiceProvider);

app.UseExceptionHandler();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumi API v1"); });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
