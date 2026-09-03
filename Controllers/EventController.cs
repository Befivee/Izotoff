using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class EventController : Controller
{
    public IActionResult Index() => RedirectToActionPermanent("Index", "Excursion");
}
