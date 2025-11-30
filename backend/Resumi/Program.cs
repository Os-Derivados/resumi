using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Resumi.Infra.Database.Context;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Minha API",
            Version = "v1",
            Description = "Descrição breve da API",
        }
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resumi API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
