using RunnerUp.Models;

namespace RunnerUp.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetAllAsync();
    Task<AppUser> GetByIdAsync(string id);
    bool Add(AppUser user);
    bool Update(AppUser user);
    bool Delete(AppUser user);
    bool Save();
}