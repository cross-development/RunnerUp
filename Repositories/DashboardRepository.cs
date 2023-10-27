using Microsoft.EntityFrameworkCore;
using RunnerUp.Data;
using RunnerUp.Models;
using RunnerUp.Extensions;
using RunnerUp.Interfaces;

namespace RunnerUp.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<Race>> GetAllUserRaces()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();

        var userRaces = await _context.Races
            .Where(club => club.AppUser.Id == currentUser)
            .ToListAsync();

        return userRaces;
    }

    public async Task<List<Club>> GetAllUserClubs()
    {
        var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();

        var userClubs = await _context.Clubs
            .Where(club => club.AppUser.Id == currentUser)
            .ToListAsync();

        return userClubs;
    }
}