using Microsoft.AspNetCore.Mvc;
using Izotoff.Services;
using Izotoff.ViewModels;

namespace Izotoff.Controllers;

public class ExcursionController(IPublicVisitCatalog visits) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["MetaDescription"] =
            "Посещение IZOTOFF: эко-ферма, сыроварня и программы на винограднике. Запись онлайн.";
        ViewData["MetaKeywords"] = "IZOTOFF, посещение фермы, экскурсия, виноградник, дегустация, Калининградская область";
        ViewData["OgType"] = "website";

        return View(new VisitIndexViewModel
        {
            Visits = await visits.GetAllAsync(cancellationToken)
        });
    }
}
