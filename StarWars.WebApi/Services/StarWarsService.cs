using Microsoft.Extensions.Caching.Memory;

namespace StarWars.WebApi.Services
{
    public class StarWarsService(HttpClient httpClient, IMemoryCache cache)
    {
        // We are assuming the External API below is expensive to consume, so we are caching the results for 5 minutes.
        // Retrieves characters from SWAPI. If a search term is provided, it will be appended as a query parameter.
        // The response is cached for 5 minutes to reduce redundant calls.

        public async Task<object> GetCharactersAsync(string? search)
        {
            string cacheKey = string.IsNullOrEmpty(search)? "swapi_characters_all" : $"swapi_characters_{search}";

            if (cache.TryGetValue(cacheKey, out object? cachedData))
            {
                return cachedData;
            }

            string url = "people/";
            if (!string.IsNullOrEmpty(search))
            {
                url += $"?search={search}";
            }

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return new { error = "Error retrieving data from SWAPI" };
            }

            var content = await response.Content.ReadAsStringAsync();

            // Cache the response for 5 minutes.
            cache.Set(cacheKey, content, TimeSpan.FromMinutes(5));

            return content;
        }
    }
}
