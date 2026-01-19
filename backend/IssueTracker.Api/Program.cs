using IssueTracker.Api.Data;
using IssueTracker.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

// CORS (Angular dev server)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Expone el documento OpenAPI nativo en /openapi/v1.json
    app.MapOpenApi();

    // UI de Swagger apuntando a ese JSON
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "IssueTracker v1");
        options.RoutePrefix = "swagger";
    });
}


// Error handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Unexpected error",
            Detail = ex?.Message
        };

        context.Response.StatusCode = problem.Status.Value;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem); // Serializa el ProblemDetails como JSON
    });
});

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); 

app.MapControllers();

// Seed (solo para demo)
await DbInitializer.SeedAsync(app.Services);

app.Run();
