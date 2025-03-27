using StarWars.DAL;
using StarWars.DAL.Repositories;
using StarWars.WebApi.Middlewares;
using StarWars.WebApi.Services;
using System.Runtime.CompilerServices;

namespace StarWars.WebApi.ServiceRegistiration;

public static class ServiceRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IRequestHistoryRepository, RequestHistoryRepository>();
        services.AddScoped<IFavoriteCharacterRepository, FavoriteCharacterRepository>();
        services.AddScoped<IDatabase, Database>();
        services.AddScoped<IStarWarsService, StarWarsService>();
        return services;
    }
}