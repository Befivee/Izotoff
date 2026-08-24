using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Controllers;

public class PrivacyController : Controller
{
    [HttpGet("/privacy")]
    public IActionResult LegacyPrivacy() => RedirectPermanent("/privacy-policy");

    [HttpGet("/privacy-policy")]
    public IActionResult PrivacyPolicy()
    {
        ViewData["Title"] = "Политика конфиденциальности";
        ViewData["MetaDescription"] =
            "Политика конфиденциальности сайта IZOTOFF: cookie, веб-аналитика, технические данные и права пользователей.";
        ViewData["MetaKeywords"] = "политика конфиденциальности, cookie, IZOTOFF";
        ViewData["BodyClass"] = "page-privacy";
        return View();
    }

    [HttpGet("/personal-data-policy")]
    public IActionResult PersonalDataPolicy()
    {
        ViewData["Title"] = "Политика обработки персональных данных";
        ViewData["MetaDescription"] =
            "Политика в отношении обработки персональных данных фермерского хозяйства IZOTOFF в соответствии с 152-ФЗ.";
        ViewData["MetaKeywords"] = "персональные данные, 152-ФЗ, IZOTOFF";
        ViewData["BodyClass"] = "page-privacy";
        return View();
    }
}
