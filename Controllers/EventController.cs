using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Izotoff.Options;
using Izotoff.Services;

namespace Izotoff.Controllers;

public class EventController(
    IHttpClientFactory httpClientFactory,
    IOptions<TelegramBotOptions> telegramOptions,
    IWebHostEnvironment environment) : Controller
{
    public IActionResult Index() => RedirectToActionPermanent("Index", "Excursion");

    [HttpGet("/event-media/{fileName}")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Media(string fileName, CancellationToken cancellationToken)
    {
        if (!NewsMediaPath.IsSafeFileName(fileName))
            return NotFound();

        var remote = await TryFetchRemoteAsync(fileName, cancellationToken);
        if (remote is not null)
            return remote;

        var localPath = Path.Combine(environment.WebRootPath, "uploads", "events", fileName);
        if (!System.IO.File.Exists(localPath))
            return NotFound();

        return PhysicalFile(localPath, NewsMediaPath.ContentType(fileName));
    }

    private async Task<IActionResult?> TryFetchRemoteAsync(string fileName, CancellationToken cancellationToken)
    {
        var telegram = telegramOptions.Value;
        var origin = telegram.RelayOrigin;
        if (string.IsNullOrWhiteSpace(origin))
            return null;

        try
        {
            var client = httpClientFactory.CreateClient("telegram_relay");
            using var request = new HttpRequestMessage(
                HttpMethod.Get,
                origin.TrimEnd('/') + "/internal/visits/files/" + Uri.EscapeDataString(fileName));
            if (!string.IsNullOrWhiteSpace(telegram.RelaySecret))
                request.Headers.TryAddWithoutValidation("X-Relay-Secret", telegram.RelaySecret.Trim());

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return null;

            var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var contentType = response.Content.Headers.ContentType?.MediaType
                              ?? NewsMediaPath.ContentType(fileName);
            return File(bytes, contentType);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return null;
        }
    }
}
