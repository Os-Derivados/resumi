using Microsoft.EntityFrameworkCore;
using Resumi.App.Data.Models;
using Resumi.Infra.Database;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

// Configure EF Core with Npgsql (PostgreSQL)
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(defaultConnection))
{
    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(defaultConnection));
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiDocumentation();
builder.Services.AddDomainServices();
builder.Services.AddDomainValidators();
builder.Services.AddEntityMappers();
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

using var seedScope = app.Services.CreateScope();
_ = await DbSeeder.SeedDatabaseAsync(seedScope.ServiceProvider);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumi API v1"); });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
