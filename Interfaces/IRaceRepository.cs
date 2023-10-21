using RunnerUp.Models;

namespace RunnerUp.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race>> GetAllAsync();
    Task<Race> GetByIdAsync(int id);
    Task<Race> GetByIdAsyncNoTracking(int id);
    Task<IEnumerable<Race>> GetRaceByCityAsync(string city);
    bool Add(Race club);
    bool Update(Race club);
    bool Delete(Race club);
    bool Save();
}