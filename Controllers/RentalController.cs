using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class RentalController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Home");
    }
}
