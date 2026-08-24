using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class NewsController : Controller
{
    public IActionResult Index()
    {
        ViewData["MetaDescription"] = "Новости IZOTOFF — семейная эко-ферма и виноградник.";
        ViewData["MetaKeywords"] = "IZOTOFF, новости, ферма, виноградник";
        return View();
    }
}
