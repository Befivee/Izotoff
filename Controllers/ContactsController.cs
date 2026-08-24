using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class ContactsController : Controller
{
    public IActionResult Index() => View();
}
