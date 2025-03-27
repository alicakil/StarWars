using Microsoft.EntityFrameworkCore;

namespace StarWars.DAL.Repositories;

public class BaseRepository<T>(AppDbContext context) : IBaseRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    private async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }
    public async void Update(T entity)
    {
        _dbSet.Update(entity);
        context.SaveChanges();
    }

    public void Remove(T entity)
    {
        if (entity == null)
            throw new EntityNotFoundException($"Entity of type {typeof(T).Name} was not found.");

        _dbSet.Remove(entity);
        context.SaveChanges();
    }
    
}