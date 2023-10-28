using Microsoft.EntityFrameworkCore;
using RunnerUp.Data;
using RunnerUp.Interfaces;
using RunnerUp.Models;

namespace RunnerUp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppUser>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<AppUser> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public bool Add(AppUser user)
    {
        _context.Add(user);

        return Save();
    }

    public bool Update(AppUser user)
    {
        _context.Update(user);

        return Save();
    }

    public bool Delete(AppUser user)
    {
        _context.Remove(user);

        return Save();
    }

    public bool Save()
    {
        var result = _context.SaveChanges();

        return result > 0;
    }
}