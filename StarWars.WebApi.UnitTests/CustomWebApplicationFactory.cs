using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StarWars.DAL;
using StarWars.WebApi.Services;

namespace StarWars.WebApi.UnitTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all registrations for DbContextOptions<AppDbContext>
            var dbContextDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                .ToList();
            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            // Replace with an in-memory database registration.
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDatabase");
            });

            // Remove all registrations for IStarWarsService.
            var swServiceDescriptors = services
                .Where(d => d.ServiceType == typeof(IStarWarsService))
                .ToList();
            foreach (var descriptor in swServiceDescriptors)
            {
                services.Remove(descriptor);
            }

            // Add our mock IStarWarsService
            var mockStarWarsService = new Mock<IStarWarsService>();
            mockStarWarsService
                .Setup(s => s.GetCharactersAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<dynamic>
                {
                    new { name = "Luke Skywalker", height = "172", mass = "77" }
                });
            services.AddSingleton(mockStarWarsService.Object);

            // Ensure the in-memory database is created.
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
