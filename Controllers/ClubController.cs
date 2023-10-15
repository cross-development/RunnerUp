using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.Models;

namespace RunnerUp.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;

    public ClubController(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var clubs = await _clubRepository.GetAllAsync();

        return View(clubs);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);

        return View(club);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Club club)
    {
        if (!ModelState.IsValid)
        {
            return View(club);
        }

        _clubRepository.Add(club);

        return RedirectToAction("Index");
    }
}