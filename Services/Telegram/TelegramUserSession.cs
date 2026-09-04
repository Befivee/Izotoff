using Izotoff.Services.Bot;

namespace Izotoff.Services.Telegram;

public class TelegramUserSession
{
    public TelegramBotState State { get; set; } = TelegramBotState.None;

    public BotScreen Screen { get; set; } = BotScreen.None;

    public int ListPage { get; set; }

    public int? EventId { get; set; }

    public int? NewsId { get; set; }

    public int? PendingDeleteBookingId { get; set; }

    public int? PendingDeleteEventId { get; set; }

    public int? PendingDeleteNewsId { get; set; }

    public List<int> PageIds { get; set; } = [];

    public string? DraftTitle { get; set; }

    public string? DraftDescription { get; set; }

    public DateTime? DraftEventDate { get; set; }

    public List<string> DraftImagePaths { get; set; } = [];

    public void Reset()
    {
        State = TelegramBotState.None;
        Screen = BotScreen.None;
        ListPage = 0;
        EventId = null;
        NewsId = null;
        PendingDeleteBookingId = null;
        PendingDeleteEventId = null;
        PendingDeleteNewsId = null;
        PageIds = [];
        DraftTitle = null;
        DraftDescription = null;
        DraftEventDate = null;
        DraftImagePaths = [];
    }
}
