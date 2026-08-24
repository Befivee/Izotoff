using Microsoft.AspNetCore.Mvc;
using Izotoff.Models;

namespace Izotoff.Controllers;

public class ExcursionController : Controller
{
    public IActionResult Index()
    {
        ViewData["MetaDescription"] =
            "Экскурсии IZOTOFF: на ферму и на виноградник. Дегустации, знакомство с животными. Запись онлайн.";
        ViewData["MetaKeywords"] = "IZOTOFF, экскурсии на ферму, виноградник, дегустация, Калининградская область";
        ViewData["OgType"] = "website";

        return View(ExcursionCatalog.All);
    }
}
