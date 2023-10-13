using Microsoft.EntityFrameworkCore;
using RunnerUp.Data;
using RunnerUp.Interfaces;
using RunnerUp.Models;

namespace RunnerUp.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;

    public ClubRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Club>> GetAllAsync()
    {
        return await _context.Clubs.ToListAsync();
    }

    public async Task<Club> GetByIdAsync(int id)
    {
        return await _context.Clubs
            .Include(club => club.Address)
            .FirstOrDefaultAsync(club => club.Id == id);
    }

    public async Task<IEnumerable<Club>> GetClubByCityAsync(string city)
    {
        return await _context.Clubs
            .Include(club => club.Address)
            .Where(club => club.Address.City.Contains(city))
            .ToListAsync();
    }

    public bool Add(Club club)
    {
        _context.Add(club);

        return Save();
    }

    public bool Update(Club club)
    {
        _context.Update(club);

        return Save();
    }

    public bool Delete(Club club)
    {
        _context.Remove(club);

        return Save();
    }

    public bool Save()
    {
        var result = _context.SaveChanges();

        return result > 0;
    }
}