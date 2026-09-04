using Izotoff.Models;

namespace Izotoff.Services;

public static class NewsMediaPath
{
    public const string UploadsPrefix = "/uploads/news/";
    public const string SiteProxyPrefix = "/news-media/";

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    public static bool IsSafeFileName(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        if (fileName != Path.GetFileName(fileName) ||
            fileName.Contains("..", StringComparison.Ordinal) ||
            fileName.Contains('/') ||
            fileName.Contains('\\'))
        {
            return false;
        }

        var extension = Path.GetExtension(fileName);
        return !string.IsNullOrEmpty(extension) && AllowedExtensions.Contains(extension);
    }

    public static bool TryGetUploadsFileName(string token, out string fileName)
    {
        fileName = string.Empty;
        if (string.IsNullOrWhiteSpace(token) ||
            token.StartsWith(News.TonePrefix, StringComparison.OrdinalIgnoreCase) ||
            !token.StartsWith(UploadsPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var candidate = token[UploadsPrefix.Length..];
        if (!IsSafeFileName(candidate))
            return false;

        fileName = candidate;
        return true;
    }

    public static string ToSiteProxyToken(string token)
    {
        if (TryGetUploadsFileName(token, out var fileName))
            return SiteProxyPrefix + fileName;

        return token;
    }

    public static string ContentType(string fileName) =>
        Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => "image/jpeg"
        };
}
