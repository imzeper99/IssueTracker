using IssueTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI NATIVO (.NET 9+)
builder.Services.AddOpenApi();

// EF Core SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var conn = builder.Configuration.GetConnectionString("Default");
    opt.UseSqlite(conn);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Expone el documento OpenAPI generado por ASP.NET Core
    app.MapOpenApi(); // => /openapi/v1.json por defecto

    // Swagger UI (solo interfaz). Apunta al OpenAPI nativo
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "IssueTracker v1");
        options.RoutePrefix = "swagger"; // => /swagger
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
