using StarWars.DAL.Entities;

namespace StarWars.DAL.Repositories;

public class FavoriteCharacterRepository(AppDbContext context) : BaseRepository<FavoriteCharacter>(context), IFavoriteCharacterRepository
{
}
