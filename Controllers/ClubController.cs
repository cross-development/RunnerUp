using Microsoft.AspNetCore.Mvc;

namespace RunnerUp.Controllers;

public class ClubController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}