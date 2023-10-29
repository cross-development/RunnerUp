using System.Diagnostics;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RunnerUp.Helpers;
using RunnerUp.Interfaces;
using RunnerUp.Models;
using RunnerUp.ViewModels;

namespace RunnerUp.Controllers;

public class HomeController : Controller
{
    private readonly IpInfoSettings _ipInfoSettings;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IClubRepository _clubRepository;

    public HomeController(
        IHttpClientFactory clientFactory, 
        IOptions<IpInfoSettings> config, 
        IClubRepository clubRepository)
    {
        _ipInfoSettings = config.Value;
        _clientFactory = clientFactory;
        _clubRepository = clubRepository;
    }

    public async Task<IActionResult> Index()
    {
        var homeViewModel = new HomeViewModel();

        try
        {
            var client = _clientFactory.CreateClient();


            var url = $"https://ipinfo.io?token={_ipInfoSettings.Token}";
            var httpReqMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(url));

            var result = await client.SendAsync(httpReqMessage);
            var info = await result.Content.ReadAsStringAsync();

            IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);

            RegionInfo myRegion = new RegionInfo(ipInfo.Country);
            ipInfo.Country = myRegion.EnglishName;

            homeViewModel.City = ipInfo.City;
            homeViewModel.State = ipInfo.Region;

            if (homeViewModel.City != null)
            {
                homeViewModel.Clubs = await _clubRepository.GetClubByCityAsync(homeViewModel.City);
            }
            else
            {
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);

        }
        catch (Exception)
        {
            homeViewModel.Clubs = null;
        }

        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
