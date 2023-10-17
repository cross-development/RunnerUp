using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.Models;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;

    public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
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
    public async Task<IActionResult> Create(CreateRaceViewModel raceViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(raceViewModel.Image);

            var race = new Race
            {
                Title = raceViewModel.Title,
                Description = raceViewModel.Description,
                Image = result.Url.ToString(),
                Address = new Address
                {
                    Street = raceViewModel.Address.Street,
                    City = raceViewModel.Address.City,
                    State = raceViewModel.Address.State
                }
            };

            _raceRepository.Add(race);

            return RedirectToAction("Index");

        }
        else
        {
            ModelState.AddModelError("error", "Photo uploaded failed");
        }

        return View(raceViewModel);
    }
}
