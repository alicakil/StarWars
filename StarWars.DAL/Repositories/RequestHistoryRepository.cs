using StarWars.DAL.Entities;

namespace StarWars.DAL.Repositories;

public class RequestHistoryRepository(AppDbContext context) : BaseRepository<RequestHistory>(context), IRequestHistoryRepository
{
}