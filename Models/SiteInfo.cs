using System.Globalization;

namespace Izotoff.Models;

public static class SiteInfo
{
    public const string CastleName = "IZOTOFF";
    public const string OperatorName = "Фермерское хозяйство «Изотов» (IZOTOFF CHEESE)";
    public const string PolicyPublishedDate = "УТОЧНЯЕТСЯ";
    public const string SiteUrl = "https://изотофф39.рф";
    public const string PartnerName = "Замок Вальдау";
    public const string PartnerUrl = "https://вальдау.рф";
    public const string PartnerLead =
        "Средневековая крепость XIII века в посёлке Низовье — экскурсии, мероприятия и живая история.";
    public const string PartnerCta = "Перейти на сайт замка";
    public const string HomePageTitle = "IZOTOFF — официальный сайт";
    public const string BrowserTitle = HomePageTitle;
    public const string Tagline = "Первый и самый большой виноградник Калининградской области";
    public const string Location = "Калининградская область, Зеленоградский район";
    public const string Address = "Калининградская обл., Зеленоградский район (точный адрес — УТОЧНЯЕТСЯ)";
    public const string Phone = "ТЕЛЕФОН УТОЧНЯЕТСЯ";
    public const string PhoneTel = "+70000000000";
    public const string Phone2 = "";
    public const string Phone2Tel = "";
    public const string VkUrl = "https://vk.com/TODO";
    public const string VkLabel = "ВКонтакте — УТОЧНЯЕТСЯ";
    public const string TelegramUrl = "https://t.me/TODO";
    public const string TelegramLabel = "Telegram — УТОЧНЯЕТСЯ";
    public const string WorkingHours = "ПО ЗАПИСИ (расписание уточняется)";
    public const string TicketPrice = "от 500 ₽";
    public const string BusRoute = "На автомобиле";
    public const string DistanceFromKg = "~30 мин от Калининграда";
    public const string TravelTime = "Эко-ферма в Зеленоградском районе";
    public const double Latitude = 54.962;
    public const double Longitude = 20.476;

    public static string GetDocumentTitle(string? pageTitle, string? metaTitle) =>
        !string.IsNullOrWhiteSpace(metaTitle)
            ? metaTitle.Trim()
            : string.IsNullOrWhiteSpace(pageTitle) || pageTitle == "Главная"
                ? HomePageTitle
                : $"{pageTitle.Trim()} — {HomePageTitle}";

    public static string GetBrowserTitle(string? pageTitle) =>
        GetDocumentTitle(pageTitle, null);

    public static string YandexMapsUrl =>
        $"https://yandex.ru/maps/?text={Uri.EscapeDataString(Address)}&pt={Longitude.ToString(CultureInfo.InvariantCulture)},{Latitude.ToString(CultureInfo.InvariantCulture)}&z=16&l=map";

    public static string TwoGisUrl =>
        $"https://2gis.ru/geo/{Longitude.ToString(CultureInfo.InvariantCulture)},{Latitude.ToString(CultureInfo.InvariantCulture)}?m={Longitude.ToString(CultureInfo.InvariantCulture)},{Latitude.ToString(CultureInfo.InvariantCulture)},16&query={Uri.EscapeDataString(Address)}";

    public static string YandexMapEmbedUrl =>
        $"https://yandex.ru/map-widget/v1/?ll={Longitude.ToString(CultureInfo.InvariantCulture)}%2C{Latitude.ToString(CultureInfo.InvariantCulture)}&z=14&l=map&pt={Longitude.ToString(CultureInfo.InvariantCulture)}%2C{Latitude.ToString(CultureInfo.InvariantCulture)}";
}
