namespace StarWars.DAL.Entities;

public class FavoriteCharacter
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ExternalId { get; set; }
}
