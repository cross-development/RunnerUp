using RunnerUp.Models;

namespace RunnerUp.Interfaces;

public interface IDashboardRepository
{
    Task<List<Race>> GetAllUserRaces();
    Task<List<Club>> GetAllUserClubs();
}