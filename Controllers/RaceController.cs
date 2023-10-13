using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;

namespace RunnerUp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;

    public RaceController(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;
    }

    public async Task<IActionResult> Index()
    {
        var races = await _raceRepository.GetAllAsync();

        return View(races);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);

        return View(race);
    }
}
