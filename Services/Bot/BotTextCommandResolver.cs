using System.Globalization;

namespace Izotoff.Services.Bot;

public static class BotTextCommandResolver
{
    private const int EventDetailButtonCount = 4;
    private const int NewsDetailButtonCount = 5;
    private const int MainMenuButtonCount = 4;

    public static bool TryResolve(
        string? text,
        BotScreen screen,
        IReadOnlyList<int> pageIds,
        out string payload)
    {
        payload = string.Empty;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var normalized = text.Trim();

        if (normalized == BotReplyLabels.BackMain)
        {
            payload = BotCallbackData.MenuMain;
            return true;
        }

        if (normalized == BotReplyLabels.Back)
        {
            payload = screen switch
            {
                BotScreen.EventDetail => BotCallbackData.EventBackList,
                BotScreen.NewsDetail => BotCallbackData.NewsBackList,
                _ => BotCallbackData.MenuMain
            };
            return true;
        }

        if (normalized == BotReplyLabels.Add)
        {
            payload = screen switch
            {
                BotScreen.Events => BotCallbackData.EventAdd,
                BotScreen.News => BotCallbackData.NewsAdd,
                _ => string.Empty
            };
            return payload.Length > 0;
        }

        if (normalized == BotReplyLabels.Prev)
        {
            payload = BotCallbackData.PagePrev;
            return screen is BotScreen.Bookings or BotScreen.Events or BotScreen.News;
        }

        if (normalized == BotReplyLabels.Next)
        {
            payload = BotCallbackData.PageNext;
            return screen is BotScreen.Bookings or BotScreen.Events or BotScreen.News;
        }

        if (normalized == BotReplyLabels.Yes || normalized == BotReplyLabels.No)
            return false;

        if (int.TryParse(normalized, NumberStyles.None, CultureInfo.InvariantCulture, out var index) &&
            IsValidMenuIndex(screen, index, pageIds.Count))
        {
            payload = screen switch
            {
                BotScreen.Bookings when pageIds.Count >= index =>
                    BotCallbackData.BookingDelete(pageIds[index - 1]),
                BotScreen.Events when pageIds.Count >= index =>
                    BotCallbackData.EventView(pageIds[index - 1]),
                BotScreen.News when pageIds.Count >= index =>
                    BotCallbackData.NewsView(pageIds[index - 1]),
                BotScreen.EventDetail when pageIds.Count > 0 => index switch
                {
                    1 => BotCallbackData.EventEditTitle(pageIds[0]),
                    2 => BotCallbackData.EventEditDescription(pageIds[0]),
                    3 => BotCallbackData.EventEditImage(pageIds[0]),
                    4 => BotCallbackData.EventDelete(pageIds[0]),
                    _ => string.Empty
                },
                BotScreen.NewsDetail when pageIds.Count > 0 => index switch
                {
                    1 => BotCallbackData.NewsEditTitle(pageIds[0]),
                    2 => BotCallbackData.NewsEditDescription(pageIds[0]),
                    3 => BotCallbackData.NewsEditDate(pageIds[0]),
                    4 => BotCallbackData.NewsEditImage(pageIds[0]),
                    5 => BotCallbackData.NewsDelete(pageIds[0]),
                    _ => string.Empty
                },
                BotScreen.Main or BotScreen.None => index switch
                {
                    1 => BotCallbackData.MenuBookings,
                    2 => BotCallbackData.MenuEvents,
                    3 => BotCallbackData.MenuNews,
                    4 => BotCallbackData.MenuStats,
                    _ => string.Empty
                },
                _ => string.Empty
            };

            return payload.Length > 0;
        }

        if (screen is BotScreen.Main or BotScreen.None)
        {
            payload = normalized switch
            {
                "1" => BotCallbackData.MenuBookings,
                "2" => BotCallbackData.MenuEvents,
                "3" => BotCallbackData.MenuNews,
                "4" => BotCallbackData.MenuStats,
                _ => string.Empty
            };
            return payload.Length > 0;
        }

        return false;
    }

    private static bool IsValidMenuIndex(BotScreen screen, int index, int pageIdsCount) =>
        screen switch
        {
            BotScreen.Bookings or BotScreen.Events or BotScreen.News =>
                index is >= 1 and <= BotListPaging.PageSize && pageIdsCount >= index,
            BotScreen.EventDetail =>
                index is >= 1 and <= EventDetailButtonCount && pageIdsCount > 0,
            BotScreen.NewsDetail =>
                index is >= 1 and <= NewsDetailButtonCount && pageIdsCount > 0,
            BotScreen.Main or BotScreen.None =>
                index is >= 1 and <= MainMenuButtonCount,
            _ => false
        };

    public static bool TryResolveConfirmation(string? text, out bool confirmed)
    {
        confirmed = false;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        var normalized = text.Trim();
        if (normalized == BotReplyLabels.Yes)
        {
            confirmed = true;
            return true;
        }

        if (normalized == BotReplyLabels.No)
        {
            confirmed = false;
            return true;
        }

        return false;
    }
}
