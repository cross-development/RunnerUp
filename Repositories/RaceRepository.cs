using Microsoft.EntityFrameworkCore;
using RunnerUp.Data;
using RunnerUp.Interfaces;
using RunnerUp.Models;

namespace RunnerUp.Repositories;

public class RaceRepository : IRaceRepository
{
    private readonly ApplicationDbContext _context;

    public RaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Race>> GetAllAsync()
    {
        return await _context.Races.ToListAsync();
    }

    public async Task<Race> GetByIdAsync(int id)
    {
        return await _context.Races
            .Include(race => race.Address)
            .FirstOrDefaultAsync(race => race.Id == id);
    }

    public async Task<IEnumerable<Race>> GetRaceByCityAsync(string city)
    {
        return await _context.Races
            .Include(race => race.Address)
            .Where(race => race.Address.City.Contains(city))
            .ToListAsync();
    }

    public bool Add(Race race)
    {
        _context.Add(race);

        return Save();
    }

    public bool Update(Race race)
    {
        _context.Update(race);

        return Save();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);

        return Save();
    }

    public bool Save()
    {
        var result = _context.SaveChanges();

        return result > 0;
    }
}