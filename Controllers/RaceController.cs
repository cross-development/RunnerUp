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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);

        if (race == null)
        {
            return View("Error");
        }

        var raceViewModel = new EditRaceViewModel
        {
            Title = race.Title,
            Description = race.Description,
            AddressId = race.AddressId,
            Address = race.Address,
            Url = race.Image,
            RaceCategory = race.RaceCategory
        };

        return View(raceViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel editRaceViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Failed to edit race");

            return View("Edit", editRaceViewModel);
        }

        var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

        if (userRace == null)
        {
            return View(editRaceViewModel);
        }

        try
        {
            await _photoService.DeletePhotoAsync(userRace.Image);
        }
        catch (Exception)
        {
            ModelState.AddModelError("error", "Could not delete photo");

            return View(editRaceViewModel);
        }

        var photoResult = await _photoService.AddPhotoAsync(editRaceViewModel.Image);

        var race = new Race
        {
            Id = id,
            Title = editRaceViewModel.Title,
            Description = editRaceViewModel.Description,
            Image = photoResult.Url.ToString(),
            AddressId = editRaceViewModel.AddressId,
            Address = editRaceViewModel.Address
        };

        _raceRepository.Update(race);

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var raceDetails = await _raceRepository.GetByIdAsync(id);

        return raceDetails == null ? View("Error") : View(raceDetails);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteRace(int id)
    {
        var raceDetails = await _raceRepository.GetByIdAsync(id);

        if (raceDetails == null)
        {
            return View("Error");
        }

        _raceRepository.Delete(raceDetails);

        return RedirectToAction("Index");
    }
}
