using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace StarWars.WebApi.Services;

public interface IStarWarsService
{
    Task<IEnumerable<dynamic>> GetCharactersAsync(string? searchTerm = null);
}

public class StarWarsService : IStarWarsService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;

    public StarWarsService(HttpClient httpClient, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    public virtual async Task<IEnumerable<dynamic>> GetCharactersAsync(string? searchTerm = null)
    {
        // Create a cache key based on the search term
        string cacheKey = $"characters_{searchTerm ?? "all"}";

        // Try to get from cache first
        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<dynamic>? cachedCharacters) && cachedCharacters != null)
        {
            return cachedCharacters;
        }

        // If not in cache, fetch from SWAPI
        var response = await _httpClient.GetFromJsonAsync<dynamic>("people/");

        var characters = (IEnumerable<dynamic>?)response?.results ?? new List<dynamic>();

        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            characters = characters.Where(c =>
                c.name.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        // Cache the results for 5 minutes
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        _memoryCache.Set(cacheKey, characters, cacheEntryOptions);

        return characters;
    }
}