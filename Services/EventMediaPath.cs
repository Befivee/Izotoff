namespace Izotoff.Services;

public static class EventMediaPath
{
    public const string UploadsPrefix = "/uploads/events/";
    public const string SiteProxyPrefix = "/event-media/";

    public static bool TryGetUploadsFileName(string? token, out string fileName)
    {
        fileName = string.Empty;
        if (string.IsNullOrWhiteSpace(token) ||
            !token.StartsWith(UploadsPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var candidate = token[UploadsPrefix.Length..];
        if (!NewsMediaPath.IsSafeFileName(candidate))
            return false;

        fileName = candidate;
        return true;
    }

    public static string ToSiteProxyToken(string? token)
    {
        if (TryGetUploadsFileName(token, out var fileName))
            return SiteProxyPrefix + fileName;

        return token ?? string.Empty;
    }
}
