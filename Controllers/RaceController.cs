using Microsoft.AspNetCore.Mvc;

namespace RunnerUp.Controllers;

public class RaceController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
