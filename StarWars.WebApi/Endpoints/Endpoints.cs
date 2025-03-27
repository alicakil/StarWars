using StarWars.DAL;
using StarWars.DAL.Entities;
using StarWars.WebApi.Services;

namespace StarWars.WebApi.Endpoints;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        // GET /api/v1/characters?search={searchTerm}
        // Retrieves character data from SWAPI; optionally filtered by a search term.
        // The result is cached in memory for 5 minutes.
        app.MapGet("/api/v1/characters", async (string? search, IStarWarsService swService) =>
        {
            var result = await swService.GetCharactersAsync(search);
            return Results.Ok(result);
        })
        .WithName("GetCharacters")
        .Produces(200);

        // POST /api/v1/favorites
        // Adds a favorite character to the database.
        app.MapPost("/api/v1/favorites", async (FavoriteCharacter favorite, IDatabase db) =>
        {
            await db.FavoriteCharacterRepository.AddAsync(favorite);
            return Results.Created($"/api/v1/favorites/{favorite.Id}", favorite);
        })
        .WithName("AddFavorite");

        // DELETE /api/v1/favorites/{id}
        // Removes a favorite character from the database.
        app.MapDelete("/api/v1/favorites/{id}", async (int id, IDatabase db) =>
        {
            var favorite = await db.FavoriteCharacterRepository.GetByIdAsync(id);
            if (favorite == null)
            {
                return Results.NotFound();
            }

            db.FavoriteCharacterRepository.Remove(favorite);
            return Results.NoContent();
        })
        .WithName("DeleteFavorite");

        // GET /api/v1/favorites
        // Lists all favorite characters.
        app.MapGet("/api/v1/favorites", async (IDatabase db) =>
        {
            var favorites = await db.FavoriteCharacterRepository.GetAllAsync();
            return Results.Ok(favorites);
        })
        .WithName("GetFavorites");

        // GET /api/v1/history
        // Returns the API request history.
        app.MapGet("/api/v1/history", async (IDatabase db) =>
        {
            var history = (await db.RequestHistoryRepository.GetAllAsync())
                .OrderByDescending(h => h.RequestedAt)
                .ToList();
            return Results.Ok(history);
        })
        .WithName("GetHistory");
    }
}
