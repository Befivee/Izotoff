using Microsoft.AspNetCore.Mvc;
using Izotoff.Models;
using Izotoff.Services;
using Izotoff.ViewModels;

namespace Izotoff.Controllers;

public class HomeController(IPublicVisitCatalog visits, IPublicNewsCatalog news) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["MetaTitle"] = SiteInfo.HomePageTitle;
        ViewData["MetaDescription"] =
            "IZOTOFF — семейная экоферма и виноградник в Калининградской области: сыроварня, дегустации, экскурсии на ферму. Зеленоградский район, запись онлайн.";
        ViewData["MetaKeywords"] =
            "IZOTOFF, Изотов, экоферма, эко-ферма, виноградник, ферма, сыроварня, Калининградская область, дегустация, экскурсии";
        ViewData["OgType"] = "website";
        ViewData["OgImage"] = null;
        ViewData["BodyClass"] = "page-home";

        var latestNews = await news.GetLatestAsync(3, cancellationToken);
        var allVisits = await visits.GetAllAsync(cancellationToken);
        var model = new HomeIndexViewModel
        {
            FeaturedNews = latestNews.Select(item => item.ToHomeItem()).ToList(),
            UpcomingEvents = allVisits.Take(3).ToList()
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
