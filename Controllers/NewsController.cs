using Microsoft.AspNetCore.Mvc;
using Izotoff.Services;

namespace Izotoff.Controllers;

public class NewsController(INewsService news) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewData["MetaDescription"] = "Новости IZOTOFF — семейная эко-ферма и виноградник.";
        ViewData["MetaKeywords"] = "IZOTOFF, новости, ферма, виноградник";
        var items = await news.GetAllAsync(cancellationToken);
        return View(items.Select(item => item.ToHomeItem()).ToList());
    }
}
