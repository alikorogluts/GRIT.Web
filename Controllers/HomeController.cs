using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GRIT.Web.Models;
using GRIT.Web.Services;
using Microsoft.AspNetCore.Localization;

namespace GRIT.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

    public async Task<IActionResult> Index()
    {
        var culture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

        var newsList = await _homeService.GetHomeNewsAsync(culture);

        return View(newsList);
    }

    [HttpPost]
    public IActionResult ChangeLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}