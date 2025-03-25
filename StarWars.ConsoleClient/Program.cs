using System.Net.Http.Json;

namespace StarWars.ConsoleClient;

public class Program
{
    static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();
        var apiClient = new StarWarsApiClient(httpClient);

        while (true)
        {
            Console.WriteLine("\nStar Wars Character Explorer");
            Console.WriteLine("1. List Characters");
            Console.WriteLine("2. Search Characters");
            Console.WriteLine("3. Add Favorite Character");
            Console.WriteLine("4. Remove Favorite Character");
            Console.WriteLine("5. View Favorite Characters");
            Console.WriteLine("6. View Request History");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await apiClient.ListCharactersAsync();
                        break;
                    case "2":
                        Console.Write("Enter search term: ");
                        var searchTerm = Console.ReadLine();
                        await apiClient.ListCharactersAsync(searchTerm);
                        break;
                    case "3":
                        Console.Write("Enter character name: ");
                        var name = Console.ReadLine();
                        Console.Write("Enter character height: ");
                        var height = Console.ReadLine();
                        Console.Write("Enter character mass: ");
                        var mass = Console.ReadLine();

                        var character = new CharacterDto
                        {
                            Name = name,
                            Height = height,
                            Mass = mass
                        };
                        await apiClient.AddFavoriteCharacterAsync(character);
                        break;
                    case "4":
                        Console.Write("Enter favorite character ID to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int favoriteId))
                        {
                            await apiClient.RemoveFavoriteCharacterAsync(favoriteId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        break;
                    case "5":
                        await apiClient.ListFavoriteCharactersAsync();
                        break;
                    case "6":
                        await apiClient.ViewRequestHistoryAsync();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}