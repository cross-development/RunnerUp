using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.Models;

namespace RunnerUp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;

    public RaceController(IRaceRepository raceRepository)
    {
        _raceRepository = raceRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var races = await _raceRepository.GetAllAsync();

        return View(races);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);

        return View(race);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Race race)
    {
        if (!ModelState.IsValid)
        {
            return View(race);
        }

        _raceRepository.Add(race);

        return RedirectToAction("Index");
    }
}
