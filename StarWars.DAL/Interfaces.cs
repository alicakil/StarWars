using StarWars.DAL.Entities;

namespace StarWars.DAL;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}

public interface IFavoriteCharacterRepository : IBaseRepository<FavoriteCharacter> { }
public interface IRequestHistoryRepository : IBaseRepository<RequestHistory> { }

/// <summary>
/// Database class is a facade for all repositories. It is used to simplify the process of accessing repositories.
/// Lazy initialization of repositories.(only when they are needed)
/// </summary>
public interface IDatabase
{
    IRequestHistoryRepository RequestHistoryRepository { get; }
    IFavoriteCharacterRepository FavoriteCharacterRepository { get; }

    // optional design: Complete() method can be implement here to use Unit of Work pattern. (in my opinion not needed)
}
