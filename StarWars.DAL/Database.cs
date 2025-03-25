using StarWars.DAL.Repositories;

namespace StarWars.DAL;

public class Database(AppDbContext context) : IDatabase
{
    private readonly Lazy<IRequestHistoryRepository> _requestHistoryRepository = new(() => new RequestHistoryRepository(context));
    private readonly Lazy<IFavoriteCharacterRepository> _favoriteCharacterRepository = new(() => new FavoriteCharacterRepository(context));

    public IRequestHistoryRepository RequestHistoryRepository => _requestHistoryRepository.Value;
    public IFavoriteCharacterRepository FavoriteCharacterRepository => _favoriteCharacterRepository.Value;
}