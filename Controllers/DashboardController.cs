using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userRaces = await _dashboardRepository.GetAllUserRaces();
        var userClubs = await _dashboardRepository.GetAllUserClubs();

        var dashboardViewModel = new DashboardViewModel
        {
            Clubs = userClubs,
            Races = userRaces
        };

        return View(dashboardViewModel);
    }
}
