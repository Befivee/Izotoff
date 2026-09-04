using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Izotoff.Models;
using Izotoff.Options;

namespace Izotoff.Services;

public interface IPublicNewsCatalog
{
    Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<News>> GetLatestAsync(int count, CancellationToken cancellationToken = default);
}

/// <summary>Timeweb reads news from the Hostkey bot DB; locally uses SQLite.</summary>
public class PublicNewsCatalog(
    INewsService news,
    IHttpClientFactory httpClientFactory,
    IOptions<TelegramBotOptions> options,
    ILogger<PublicNewsCatalog> logger) : IPublicNewsCatalog
{
    public async Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var remote = await TryFetchRemoteAsync(cancellationToken);
        return remote ?? await news.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<News>> GetLatestAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count <= 0)
            count = 3;

        return (await GetAllAsync(cancellationToken)).Take(count).ToList();
    }

    private async Task<IReadOnlyList<News>?> TryFetchRemoteAsync(CancellationToken cancellationToken)
    {
        var telegram = options.Value;
        var origin = telegram.RelayOrigin;
        if (string.IsNullOrWhiteSpace(origin))
            return null;

        try
        {
            var client = httpClientFactory.CreateClient("telegram_relay");
            using var request = new HttpRequestMessage(HttpMethod.Get, origin.TrimEnd('/') + "/internal/news");
            if (!string.IsNullOrWhiteSpace(telegram.RelaySecret))
                request.Headers.TryAddWithoutValidation("X-Relay-Secret", telegram.RelaySecret.Trim());

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Каталог новостей с Hostkey: {StatusCode}.", (int)response.StatusCode);
                return null;
            }

            var payload = await response.Content.ReadFromJsonAsync<List<NewsRelayDto>>(cancellationToken);
            if (payload is null)
                return [];

            return payload
                .Select(item => item.ToNews())
                .OrderByDescending(item => item.PublishedAt)
                .ThenByDescending(item => item.Id)
                .ToList();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Не удалось загрузить новости с Hostkey.");
            return null;
        }
    }
}

public sealed class NewsRelayDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("publishedAt")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("imagePaths")]
    public string ImagePaths { get; set; } = string.Empty;

    public static NewsRelayDto From(News entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Summary = entity.Summary,
        PublishedAt = entity.PublishedAt,
        ImagePaths = entity.ImagePaths
    };

    public News ToNews()
    {
        var tokens = string.IsNullOrWhiteSpace(ImagePaths)
            ? []
            : ImagePaths
                .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(NewsMediaPath.ToSiteProxyToken)
                .Take(News.MaxImages);

        return new News
        {
            Id = Id,
            Title = Title,
            Summary = Summary,
            PublishedAt = PublishedAt,
            ImagePaths = string.Join(';', tokens)
        };
    }
}
