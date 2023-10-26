using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunnerUp.Data;
using RunnerUp.Models;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(
        ApplicationDbContext context, 
        SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var response = new LoginViewModel();

        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

        if (user != null)
        {
            // User is found, check password
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (passwordCheck)
            {
                // Password is correct, sign in
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Race");
                }
            }

            // Password is incorrect
            TempData["Error"] = "Wrong credentials. Please, try again";

            return View(loginViewModel);
        }

        // User not found
        TempData["Error"] = "Wrong credentials. Please, try again";

        return View(loginViewModel);
    }
}
