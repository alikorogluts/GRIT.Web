using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GRIT.Web.Models;

namespace GRIT.Web.Controllers;

public class CorporateController : Controller
{
    private readonly ILogger<CorporateController> _logger;

    public CorporateController(ILogger<CorporateController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
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