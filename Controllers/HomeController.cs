using Microsoft.AspNetCore.Mvc;
using Izotoff.Models;
using Izotoff.Services;
using Izotoff.ViewModels;

namespace Izotoff.Controllers;

public class HomeController(IEventService events) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["MetaTitle"] = SiteInfo.HomePageTitle;
        ViewData["MetaDescription"] =
            "IZOTOFF — семейная эко-ферма и первый виноградник Калининградской области. Сыроварня, дегустации, экскурсии и мероприятия. Запись онлайн.";
        ViewData["MetaKeywords"] =
            "IZOTOFF, Изотов, сыроварня, виноградник, Калининградская область, эко-ферма, дегустация, экскурсии, винные туры";
        ViewData["OgType"] = "website";
        ViewData["OgImage"] = null;
        ViewData["BodyClass"] = "page-home";

        var model = new HomeIndexViewModel
        {
            FeaturedNews = HomeNewsCatalog.Featured,
            UpcomingEvents = await events.GetUpcomingAsync(3, cancellationToken)
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public new IActionResult NotFound()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        ViewData["Title"] = "Страница не найдена";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult ServerError()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        ViewData["Title"] = "Ошибка сервера";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult StatusCodeError(int statusCode)
    {
        Response.StatusCode = statusCode;
        return statusCode switch
        {
            StatusCodes.Status404NotFound => View("NotFound"),
            >= 500 and < 600 => View("ServerError"),
            _ => View("Error")
        };
    }
}
