using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class RentalController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Аренда";
        ViewData["MetaDescription"] = "Аренда виноградников IZOTOFF.";
        ViewData["MetaKeywords"] = "IZOTOFF, аренда виноградника, Калининградская область";
        return View();
    }
}
