using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMS.Web.Models;

namespace CMS.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult About()
    {
        var about = new AboutViewModel
        {
            Title = "About",
            Message = "For MScProject, This Project aims to illustrate a Carer / Patient Management System.",
            Formed = new DateTime(2023, 05, 24)
        };
        return View(about);
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
