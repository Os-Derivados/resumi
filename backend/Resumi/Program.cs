using Microsoft.EntityFrameworkCore;
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumi API v1"); });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
