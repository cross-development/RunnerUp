using Microsoft.AspNetCore.Mvc;
using RunnerUp.Interfaces;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("users")]
    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllAsync();

        var usersMap = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Pace = user.Pace,
            Mileage = user.Mileage
        }).ToList();

        return View(usersMap);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        var userDetailViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Mileage = user.Mileage,
            Pace = user.Pace
        };

        return View(userDetailViewModel);
    }
}
