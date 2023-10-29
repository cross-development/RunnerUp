using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunnerUp.Extensions;
using RunnerUp.Interfaces;
using RunnerUp.Models;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class DashboardController : Controller
{
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(
        IPhotoService photoService,
        IHttpContextAccessor httpContextAccessor,
        IDashboardRepository dashboardRepository)
    {
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
        _dashboardRepository = dashboardRepository;
    }

    private void MapUserEdit(AppUser user, EditUserDashboardViewModel editUserViewModel, ImageUploadResult photoResult)
    {
        user.Id = editUserViewModel.Id;
        user.Pace = editUserViewModel.Pace;
        user.Mileage = editUserViewModel.Mileage;
        user.ProfileImageUrl = photoResult.Url.ToString();
        user.Address = new Address
        {
            State = editUserViewModel.State,
            Street = editUserViewModel.Street,
            City = editUserViewModel.City
        };
    }

    [HttpGet]
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

    [HttpGet]
    public async Task<IActionResult> EditUserProfile()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var user = await _dashboardRepository.GetUserById(currentUserId);

        if (user == null)
        {
            return View("Error");
        }

        var editUserViewModel = new EditUserDashboardViewModel
        {
            Id = currentUserId,
            Pace = user.Pace,
            Mileage = user.Mileage,
            ProfileImageUrl = user.ProfileImageUrl,
            City = user.Address.City,
            State = user.Address.State
        };

        return View(editUserViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editUserDashboardViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("error", "Failed to edit profile");

            return View("EditUserProfile", editUserDashboardViewModel);
        }

        var user = await _dashboardRepository.GetUserByIdNoTracking(editUserDashboardViewModel.Id);

        if (string.IsNullOrWhiteSpace(user.ProfileImageUrl))
        {
            var photoResult = await _photoService.AddPhotoAsync(editUserDashboardViewModel.Image);

            MapUserEdit(user, editUserDashboardViewModel, photoResult);

            _dashboardRepository.Update(user);

            return RedirectToAction("Index", "Home");
        }
        else
        {
            try
            {
                await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
            }
            catch (Exception)
            {
                ModelState.AddModelError("error", "Could not delete photo");

                return View(editUserDashboardViewModel);
            }

            var photoResult = await _photoService.AddPhotoAsync(editUserDashboardViewModel.Image);

            MapUserEdit(user, editUserDashboardViewModel, photoResult);

            _dashboardRepository.Update(user);

            return RedirectToAction("Index");
        }
    }
}
