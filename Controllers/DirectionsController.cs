using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class DirectionsController : Controller
{
    public IActionResult Index() => RedirectToActionPermanent("Index", "Contacts");
}
