using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StarWars.DAL;
using StarWars.WebApi.Endpoints;
using StarWars.WebApi.Middlewares;
using StarWars.WebApi.ServiceRegistiration;
using StarWars.WebApi.Services;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Register in-memory caching service
builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100; // Maximum number of requests allowed.
        limiterOptions.Window = TimeSpan.FromMinutes(1); // Time window.
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 10; // Maximum queued requests.
    });
});

// Register HttpClient for SWAPI integration
builder.Services.AddHttpClient<IStarWarsService, StarWarsService>(client => 
{
    client.BaseAddress = new Uri("https://swapi.dev/api/");
});


// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Star Wars API Microservice",
        Version = "v1"
    });
});

builder.Services.RegisterServices();

var app = builder.Build();

app.UseRateLimiter();

app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// in dev this is quick and dirty way to create the database
/// in production I would prefer to use a migration script on a deployment pipeline
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

if (db.Database.IsRelational())
{
    Console.WriteLine("Applying migrations...");
    db.Database.Migrate();
    Console.WriteLine("Migrations applied.");
}
else
{
    // For in-memory provider, just ensure the database is created.
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();

// Middleware to log each incoming API request for history tracking.
app.UseRequestLogging();

app.MapEndpoints();
app.Run();

public partial class Program { }