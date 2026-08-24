using Microsoft.AspNetCore.Mvc;

namespace Izotoff.Helpers;

public static class StaticImageCss
{
    /// <summary>Legacy helper — images removed; use CSS placeholder classes instead.</summary>
    public static string BackgroundImageSet(IUrlHelper url, string pathWithoutExtension) => string.Empty;
}
