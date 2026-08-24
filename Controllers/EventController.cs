using Microsoft.AspNetCore.Mvc;
using Izotoff.Services;

namespace Izotoff.Controllers;

public class EventController(IEventService events) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["MetaDescription"] =
            "Мероприятия IZOTOFF: винные туры, дегустации, пикники, гастрономические вечера на эко-ферме.";
        ViewData["MetaKeywords"] = "IZOTOFF, мероприятия, винные туры, дегустация, пикник, Калининград";
        ViewData["OgType"] = "website";

        var list = await events.GetAllAsync(cancellationToken);
        return View(list);
    }
}
