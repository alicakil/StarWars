using System.Net.Http.Json;

namespace StarWars.ConsoleClient;

public class StarWarsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string BaseUrl;

    public StarWarsApiClient(HttpClient httpClient)
    {
        var host = Environment.GetEnvironmentVariable("WEBAPI_HOST") ?? "webapi";
        var port = Environment.GetEnvironmentVariable("WEBAPI_PORT") ?? "8080";
        BaseUrl = $"http://{host}:{port}/api/v1";
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
    }

    public async Task ListCharactersAsync(string? searchTerm = null)
    {
        var url = searchTerm != null
            ? $"{BaseUrl}/characters?search={Uri.EscapeDataString(searchTerm)}"
            : $"{BaseUrl}/characters";

        var response = await _httpClient.GetFromJsonAsync<List<CharacterDto>>(url);

        if (response == null || !response.Any())
        {
            Console.WriteLine("No characters found.");
            return;
        }

        Console.WriteLine("Characters:");
        foreach (var character in response)
        {
            Console.WriteLine($"- {character.Name} (Height: {character.Height}, Mass: {character.Mass})");
        }
    }

    public async Task AddFavoriteCharacterAsync(CharacterDto character)
    {
        var favoriteCharacter = new FavoriteCharacterDto
        {
            Name = character.Name,
            Height = character.Height,
            Mass = character.Mass
        };

        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/favorites", favoriteCharacter);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Added {character.Name} to favorites.");
        }
        else
        {
            Console.WriteLine($"Failed to add {character.Name} to favorites.");
        }
    }

    public async Task RemoveFavoriteCharacterAsync(int favoriteId)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/favorites/{favoriteId}");

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Removed favorite character with ID {favoriteId}.");
        }
        else
        {
            Console.WriteLine($"Failed to remove favorite character with ID {favoriteId}.");
        }
    }

    public async Task ListFavoriteCharactersAsync()
    {
        var favorites = await _httpClient.GetFromJsonAsync<List<FavoriteCharacterDto>>($"{BaseUrl}/favorites");

        if (favorites == null || !favorites.Any())
        {
            Console.WriteLine("No favorite characters found.");
            return;
        }

        Console.WriteLine("Favorite Characters:");
        foreach (var favorite in favorites)
        {
            Console.WriteLine($"- ID: {favorite.Id}, Name: {favorite.Name} (Height: {favorite.Height}, Mass: {favorite.Mass})");
        }
    }

    public async Task ViewRequestHistoryAsync()
    {
        var history = await _httpClient.GetFromJsonAsync<List<RequestHistoryDto>>($"{BaseUrl}/history");

        if (history == null || !history.Any())
        {
            Console.WriteLine("No request history found.");
            return;
        }

        Console.WriteLine("Request History:");
        foreach (var request in history)
        {
            Console.WriteLine($"- Path: {request.Path}, Method: {request.Method}, Requested At: {request.RequestedAt}");
        }
    }
}
