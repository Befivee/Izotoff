using Microsoft.AspNetCore.Mvc;
using Izotoff.Models;

namespace Izotoff.Controllers;

public class NewsController : Controller
{
    public IActionResult Index()
    {
        ViewData["MetaDescription"] = "Новости IZOTOFF — семейная эко-ферма и виноградник.";
        ViewData["MetaKeywords"] = "IZOTOFF, новости, ферма, виноградник";
        return View(HomeNewsCatalog.Featured);
    }
}
