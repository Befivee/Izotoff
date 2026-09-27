namespace Izotoff.Options;

public class SiteSettings
{
    public const string SectionName = "SiteSettings";

    public const string DefaultBaseUrl = "https://изотофф39.рф";

    public string BaseUrl { get; set; } = DefaultBaseUrl;

    public string SiteName { get; set; } = "IZOTOFF — экоферма и виноградник в Калининградской области";

    public string IndexNowKey { get; set; } = string.Empty;

    public string DefaultKeywords { get; set; } =
        "IZOTOFF, Изотов, экоферма, эко-ферма, виноградник, ферма, сыроварня, Калининградская область, дегустация, экскурсии";
}
