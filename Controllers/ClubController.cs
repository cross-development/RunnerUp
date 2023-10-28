using Microsoft.AspNetCore.Mvc;
using RunnerUp.Models;
using RunnerUp.Extensions;
using RunnerUp.Interfaces;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClubController(
        IClubRepository clubRepository, 
        IPhotoService photoService, 
        IHttpContextAccessor httpContextAccessor)
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("clubs")]
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
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();

        var createClubViewModel = new CreateClubViewModel { AppUserId = currentUserId };

        return View(createClubViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel createClubViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(createClubViewModel.Image);

            var club = new Club
            {
                Title = createClubViewModel.Title,
                Description = createClubViewModel.Description,
                Image = result.Url.ToString(),
                AppUserId = createClubViewModel.AppUserId,
                Address = new Address
                {
                    Street = createClubViewModel.Address.Street,
                    City = createClubViewModel.Address.City,
                    State = createClubViewModel.Address.State
                }
            };

            _clubRepository.Add(club);

            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("error", "Photo uploaded failed");
        }

        return View(createClubViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);

        if (club == null)
        {
            return View("Error");
        }

        var clubViewModel = new EditClubViewModel
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address,
            Url = club.Image,
            ClubCategory = club.ClubCategory
        };

        return View(clubViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel editClubViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Failed to edit club");

            return View("Edit", editClubViewModel);
        }

        var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

        if (userClub == null)
        {
            return View(editClubViewModel);
        }

        try
        {
            await _photoService.DeletePhotoAsync(userClub.Image);
        }
        catch (Exception)
        {
            ModelState.AddModelError("error", "Could not delete photo");

            return View(editClubViewModel);
        }

        var photoResult = await _photoService.AddPhotoAsync(editClubViewModel.Image);

        var club = new Club
        {
            Id = id,
            Title = editClubViewModel.Title,
            Description = editClubViewModel.Description,
            Image = photoResult.Url.ToString(),
            AddressId = editClubViewModel.AddressId,
            Address = editClubViewModel.Address
        };

        _clubRepository.Update(club);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var clubDetails = await _clubRepository.GetByIdAsync(id);

        return clubDetails == null ? View("Error") : View(clubDetails);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteCub(int id)
    {
        var clubDetails = await _clubRepository.GetByIdAsync(id);

        if (clubDetails == null)
        {
            return View("Error");
        }

        _clubRepository.Delete(clubDetails);

        return RedirectToAction("Index");
    }
}