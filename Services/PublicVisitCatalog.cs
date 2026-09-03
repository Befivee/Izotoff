using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Izotoff.Models;
using Izotoff.Options;

namespace Izotoff.Services;

public interface IPublicVisitCatalog
{
    Task<IReadOnlyList<Event>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Event>> GetUpcomingAsync(int count, CancellationToken cancellationToken = default);
}

/// <summary>Timeweb reads visits from the Hostkey bot DB; locally uses SQLite.</summary>
public class PublicVisitCatalog(
    IEventService events,
    IHttpClientFactory httpClientFactory,
    IOptions<TelegramBotOptions> options,
    ILogger<PublicVisitCatalog> logger) : IPublicVisitCatalog
{
    public async Task<IReadOnlyList<Event>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var remote = await TryFetchRemoteAsync(cancellationToken);
        return remote ?? await events.GetAllAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Event>> GetUpcomingAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count <= 0)
            count = 3;

        var today = DateTime.Today;
        return (await GetAllAsync(cancellationToken))
            .Where(e => e.EventDate.Date >= today)
            .OrderBy(e => e.EventDate)
            .Take(count)
            .ToList();
    }

    private async Task<IReadOnlyList<Event>?> TryFetchRemoteAsync(CancellationToken cancellationToken)
    {
        var telegram = options.Value;
        var origin = telegram.RelayOrigin;
        if (string.IsNullOrWhiteSpace(origin))
            return null;

        try
        {
            var client = httpClientFactory.CreateClient("telegram_relay");
            using var request = new HttpRequestMessage(HttpMethod.Get, origin.TrimEnd('/') + "/internal/visits");
            if (!string.IsNullOrWhiteSpace(telegram.RelaySecret))
                request.Headers.TryAddWithoutValidation("X-Relay-Secret", telegram.RelaySecret.Trim());

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Каталог посещений с Hostkey: {StatusCode}.", (int)response.StatusCode);
                return null;
            }

            var payload = await response.Content.ReadFromJsonAsync<List<VisitRelayDto>>(cancellationToken);
            if (payload is null)
                return [];

            return payload.Select(item => item.ToEvent()).ToList();
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogWarning(ex, "Не удалось загрузить посещения с Hostkey.");
            return null;
        }
    }
}

public sealed class VisitRelayDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("eventDate")]
    public DateTime EventDate { get; set; }

    public static VisitRelayDto From(Event entity) => new()
    {
        Id = entity.Id,
        Title = entity.Title,
        Description = entity.Description,
        EventDate = entity.EventDate
    };

    public Event ToEvent() => new()
    {
        Id = Id,
        Title = Title,
        Description = Description,
        EventDate = EventDate
    };
}
