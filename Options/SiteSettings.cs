namespace Izotoff.Options;

public class SiteSettings
{
    public const string SectionName = "SiteSettings";

    public const string DefaultBaseUrl = "https://TODO-ДОМЕН.ru";

    public string BaseUrl { get; set; } = DefaultBaseUrl;

    public string SiteName { get; set; } = "IZOTOFF — официальный сайт";

    public string IndexNowKey { get; set; } = string.Empty;

    public string DefaultKeywords { get; set; } =
        "IZOTOFF, Изотов, сыроварня, виноградник, Калининградская область, эко-ферма, дегустация, экскурсии, винные туры";
}
