using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace StarWars.WebApi.Services;

public interface IStarWarsService
{
    Task<IEnumerable<dynamic>> GetCharactersAsync(string? searchTerm = null);
}

public class StarWarsService(HttpClient httpClient, IMemoryCache memoryCache) : IStarWarsService
{
    public virtual async Task<IEnumerable<dynamic>> GetCharactersAsync(string? searchTerm = null)
    {
        // Create a cache key based on the search term
        string cacheKey = $"characters_{searchTerm ?? "all"}";

        // Try to get from cache first
        if (memoryCache.TryGetValue(cacheKey, out IEnumerable<dynamic>? cachedCharacters) && cachedCharacters != null)
        {
            return cachedCharacters;
        }

        // If not in cache, fetch from SWAPI
        var response = await httpClient.GetFromJsonAsync<dynamic>("people/");

        var characters = (IEnumerable<dynamic>?)response?.results ?? [];

        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            characters = [.. characters.Where(c =>
                c.name.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            )];
        }

        // Cache the results for 5 minutes
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        memoryCache.Set(cacheKey, characters, cacheEntryOptions);

        return characters;
    }
}