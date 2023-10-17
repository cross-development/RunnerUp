using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.Models;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;

    public ClubController(IClubRepository clubRepository, IPhotoService photoService)
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
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
    public async Task<IActionResult> Create(CreateClubViewModel clubViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(clubViewModel.Image);

            var club = new Club
            {
                Title = clubViewModel.Title,
                Description = clubViewModel.Description,
                Image = result.Url.ToString(),
                Address = new Address
                {
                    Street = clubViewModel.Address.Street,
                    City = clubViewModel.Address.City,
                    State = clubViewModel.Address.State
                }
            };

            _clubRepository.Add(club);

            return RedirectToAction("Index");

        }
        else
        {
            ModelState.AddModelError("error", "Photo uploaded failed");
        }

        return View(clubViewModel);
    }
}