using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StarWars.DAL.Entities;
using StarWars.WebApi.Services;

namespace StarWars.WebApi.UnitTests;

public class CharacterEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CharacterEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCharacters_ReturnsCharacters()
    {
        // Arrange
        // Create a scope to get the service
        using var scope = _factory.Services.CreateScope();

        // Create a mock StarWarsService
        var mockStarWarsService = new Mock<IStarWarsService>();
        mockStarWarsService
            .Setup(s => s.GetCharactersAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<dynamic>
            {
                new { name = "Luke Skywalker", height = "172", mass = "77" }
            });

        // Replace the StarWarsService with the mock
        scope.ServiceProvider.GetRequiredService<IServiceCollection>()
            .AddSingleton(mockStarWarsService.Object);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/characters");
        var characters = await response.Content.ReadFromJsonAsync<List<dynamic>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        characters.Should().NotBeEmpty();
        characters[0].name.Should().Be("Luke Skywalker");
    }
}

public class FavoriteCharacterEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public FavoriteCharacterEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddFavorite_ValidCharacter_ReturnsCreatedResponse()
    {
        // Arrange
        var favoriteCharacter = new FavoriteCharacter
        {
            Name = "Luke Skywalker",
            ExternalId = 1
        };

        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/favorites", favoriteCharacter);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}