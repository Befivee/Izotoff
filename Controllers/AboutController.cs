using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class AboutController : Controller
{
    public IActionResult Index() => View();
}
