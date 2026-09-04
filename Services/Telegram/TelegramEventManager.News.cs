using Telegram.Bot;
using Telegram.Bot.Types;
using Izotoff.Models;
using Izotoff.Services.Bot;

namespace Izotoff.Services.Telegram;

public partial class TelegramEventManager
{
    private const string NewsManagementActions =
        "\n\n1. ✏ Изменить заголовок\n" +
        "2. 📝 Изменить описание\n" +
        "3. 📅 Изменить дату\n" +
        "4. 🖼 Изменить фото\n" +
        "5. 🗑 Удалить";

    public async Task SendNewsListAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.None;
        session.Screen = BotScreen.News;
        session.PendingDeleteNewsId = null;

        await SendNewsPageAsync(bot, chatId, session.ListPage, cancellationToken);
    }

    public async Task SendNewsListWithResetAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        stateService.GetOrCreate(chatId).ListPage = 0;
        await SendNewsListAsync(bot, chatId, cancellationToken);
    }

    private async Task SendNewsPageAsync(
        ITelegramBotClient bot,
        long chatId,
        int page,
        CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        var all = await news.GetAllAsync(cancellationToken);
        var totalPages = BotListPaging.TotalPages(all.Count);
        session.ListPage = Math.Clamp(page, 0, totalPages - 1);

        var paged = BotListPaging.GetPage(all, session.ListPage);
        session.PageIds = paged.Select(item => item.Id).ToList();

        var intro = all.Count == 0
            ? "📰 Новостей пока нет.\n\nНажмите «➕ Добавить»."
            : CastleAdminContentService.BuildNumberedNewsIntro(all, session.ListPage);

        await bot.SendMessage(
            chatId,
            intro,
            replyMarkup: TelegramKeyboards.NewsPage(paged, session.ListPage, totalPages),
            cancellationToken: cancellationToken);
    }

    public async Task SendNewsDetailsAsync(
        ITelegramBotClient bot,
        long chatId,
        int newsId,
        CancellationToken cancellationToken)
    {
        var entity = await news.GetByIdAsync(newsId, cancellationToken);
        if (entity is null)
        {
            await bot.SendMessage(chatId, "Новость не найдена.", cancellationToken: cancellationToken);
            await SendNewsListAsync(bot, chatId, cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.None;
        session.Screen = BotScreen.NewsDetail;
        session.NewsId = newsId;
        session.PageIds = [newsId];

        await bot.SendMessage(
            chatId,
            content.BuildNewsDetailsText(entity) + NewsManagementActions,
            replyMarkup: TelegramKeyboards.NewsManagement(),
            cancellationToken: cancellationToken);
    }

    public async Task StartAddNewsWizardAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.WaitingForNewsTitle;
        session.DraftImagePaths = [];

        await bot.SendMessage(
            chatId,
            "➕ Новая новость\n\nШаг 1 из 4\nВведите заголовок:",
            replyMarkup: TelegramKeyboards.Remove(),
            cancellationToken: cancellationToken);
    }

    public async Task StartEditNewsTitleAsync(ITelegramBotClient bot, long chatId, int newsId, CancellationToken cancellationToken)
    {
        if (!await EnsureNewsExists(bot, chatId, newsId, cancellationToken))
            return;

        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.WaitingForNewNewsTitle;
        session.NewsId = newsId;

        await bot.SendMessage(chatId, "✏ Введите новый заголовок:", replyMarkup: TelegramKeyboards.Remove(), cancellationToken: cancellationToken);
    }

    public async Task StartEditNewsDescriptionAsync(ITelegramBotClient bot, long chatId, int newsId, CancellationToken cancellationToken)
    {
        if (!await EnsureNewsExists(bot, chatId, newsId, cancellationToken))
            return;

        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.WaitingForNewNewsDescription;
        session.NewsId = newsId;

        await bot.SendMessage(chatId, "📝 Введите новое описание:", replyMarkup: TelegramKeyboards.Remove(), cancellationToken: cancellationToken);
    }

    public async Task StartEditNewsDateAsync(ITelegramBotClient bot, long chatId, int newsId, CancellationToken cancellationToken)
    {
        var entity = await news.GetByIdAsync(newsId, cancellationToken);
        if (entity is null)
        {
            await bot.SendMessage(chatId, "Новость не найдена.", cancellationToken: cancellationToken);
            await SendNewsListAsync(bot, chatId, cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.WaitingForNewNewsDate;
        session.NewsId = newsId;

        await bot.SendMessage(
            chatId,
            $"📅 Введите новую дату (например: 14.06.2026).\nСейчас: {entity.PublishedAt.ToString("dd.MM.yyyy", RuCulture)}",
            replyMarkup: TelegramKeyboards.Remove(),
            cancellationToken: cancellationToken);
    }

    public async Task StartEditNewsImagesAsync(ITelegramBotClient bot, long chatId, int newsId, CancellationToken cancellationToken)
    {
        if (!await EnsureNewsExists(bot, chatId, newsId, cancellationToken))
            return;

        var session = stateService.GetOrCreate(chatId);
        session.State = TelegramBotState.WaitingForNewNewsImages;
        session.NewsId = newsId;
        session.DraftImagePaths = [];

        await bot.SendMessage(
            chatId,
            "🖼 Отправьте от 1 до 3 фотографий. После последней нажмите «Готово».\n«-» — стандартный фон.",
            replyMarkup: TelegramKeyboards.NewsPhotoStep(),
            cancellationToken: cancellationToken);
    }

    public async Task SendNewsDeleteConfirmationAsync(
        ITelegramBotClient bot,
        long chatId,
        int newsId,
        CancellationToken cancellationToken)
    {
        var entity = await news.GetByIdAsync(newsId, cancellationToken);
        if (entity is null)
        {
            await bot.SendMessage(chatId, "Новость не найдена.", cancellationToken: cancellationToken);
            return;
        }

        await bot.SendMessage(
            chatId,
            $"🗑 Удалить новость «{entity.Title}»?",
            replyMarkup: TelegramKeyboards.DeleteConfirmation(),
            cancellationToken: cancellationToken);

        stateService.GetOrCreate(chatId).PendingDeleteNewsId = newsId;
    }

    public async Task DeleteNewsAsync(ITelegramBotClient bot, long chatId, int newsId, CancellationToken cancellationToken)
    {
        var entity = await news.GetByIdAsync(newsId, cancellationToken);
        if (entity is null)
        {
            await bot.SendMessage(chatId, "Новость уже удалена.", cancellationToken: cancellationToken);
            await SendNewsListAsync(bot, chatId, cancellationToken);
            return;
        }

        await DeleteUploadedNewsImagesAsync(entity, cancellationToken);
        await news.DeleteAsync(newsId, cancellationToken);
        stateService.GetOrCreate(chatId).Reset();

        await bot.SendMessage(chatId, "✅ Новость удалена.", cancellationToken: cancellationToken);
        await SendNewsListAsync(bot, chatId, cancellationToken);
    }

    private async Task HandleNewsWizardTitleAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (text.Length < 2 || text.Length > 200)
        {
            await bot.SendMessage(chatId, "Заголовок должен быть от 2 до 200 символов.", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        session.DraftTitle = text;
        session.State = TelegramBotState.WaitingForNewsDescription;

        await bot.SendMessage(chatId, "Шаг 2 из 4\nВведите описание:", replyMarkup: TelegramKeyboards.Remove(), cancellationToken: cancellationToken);
    }

    private async Task HandleNewsWizardDescriptionAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (text.Length < 10 || text.Length > 2000)
        {
            await bot.SendMessage(chatId, "Описание должно быть от 10 до 2000 символов.", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        session.DraftDescription = text;
        session.State = TelegramBotState.WaitingForNewsDate;

        await bot.SendMessage(
            chatId,
            "Шаг 3 из 4\nВведите дату новости (например: 14.06.2026):",
            replyMarkup: TelegramKeyboards.Remove(),
            cancellationToken: cancellationToken);
    }

    private async Task HandleNewsWizardDateAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (!TryParseDate(text, out var date))
        {
            await bot.SendMessage(chatId, "Неверный формат даты. Пример: 14.06.2026", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        session.DraftEventDate = date;
        session.State = TelegramBotState.WaitingForNewsImages;
        session.DraftImagePaths = [];

        await bot.SendMessage(
            chatId,
            "Шаг 4 из 4\nОтправьте от 1 до 3 фотографий. После последней нажмите «Готово».\n«-» — стандартный фон.",
            replyMarkup: TelegramKeyboards.NewsPhotoStep(),
            cancellationToken: cancellationToken);
    }

    private async Task HandleNewsEditTitleAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (text.Length < 2 || text.Length > 200)
        {
            await bot.SendMessage(chatId, "Заголовок должен быть от 2 до 200 символов.", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        if (session.NewsId is null)
            return;

        var entity = await news.GetByIdAsync(session.NewsId.Value, cancellationToken);
        if (entity is null)
            return;

        entity.Title = text;
        await news.UpdateAsync(entity, cancellationToken);
        session.State = TelegramBotState.None;

        await bot.SendMessage(chatId, "✅ Заголовок обновлён.", cancellationToken: cancellationToken);
        await SendNewsDetailsAsync(bot, chatId, entity.Id, cancellationToken);
    }

    private async Task HandleNewsEditDescriptionAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (text.Length < 10 || text.Length > 2000)
        {
            await bot.SendMessage(chatId, "Описание должно быть от 10 до 2000 символов.", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        if (session.NewsId is null)
            return;

        var entity = await news.GetByIdAsync(session.NewsId.Value, cancellationToken);
        if (entity is null)
            return;

        entity.Summary = text;
        await news.UpdateAsync(entity, cancellationToken);
        session.State = TelegramBotState.None;

        await bot.SendMessage(chatId, "✅ Описание обновлено.", cancellationToken: cancellationToken);
        await SendNewsDetailsAsync(bot, chatId, entity.Id, cancellationToken);
    }

    private async Task HandleNewsEditDateAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        if (!TryParseDate(text, out var date))
        {
            await bot.SendMessage(chatId, "Неверный формат даты. Пример: 14.06.2026", cancellationToken: cancellationToken);
            return;
        }

        var session = stateService.GetOrCreate(chatId);
        if (session.NewsId is null)
            return;

        var entity = await news.GetByIdAsync(session.NewsId.Value, cancellationToken);
        if (entity is null)
            return;

        entity.PublishedAt = date.Date;
        await news.UpdateAsync(entity, cancellationToken);
        session.State = TelegramBotState.None;

        await bot.SendMessage(chatId, "✅ Дата обновлена.", cancellationToken: cancellationToken);
        await SendNewsDetailsAsync(bot, chatId, entity.Id, cancellationToken);
    }

    private async Task HandleNewsImageTextAsync(ITelegramBotClient bot, long chatId, string text, CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);

        if (text == "-")
        {
            List<string> leftovers;
            lock (session)
            {
                leftovers = session.DraftImagePaths.ToList();
                session.DraftImagePaths = [];
            }

            foreach (var leftover in leftovers)
                await images.DeleteIfUploadedAsync(leftover, cancellationToken);

            await CompleteNewsImagesAsync(bot, chatId, cancellationToken);
            return;
        }

        if (string.Equals(text, BotReplyLabels.Done, StringComparison.OrdinalIgnoreCase))
        {
            await CompleteNewsImagesAsync(bot, chatId, cancellationToken);
            return;
        }

        await bot.SendMessage(
            chatId,
            "🖼 Отправьте фото, нажмите «Готово» или «-» для стандартного фона.",
            replyMarkup: TelegramKeyboards.NewsPhotoStep(),
            cancellationToken: cancellationToken);
    }

    private async Task HandleNewsPhotoAsync(
        ITelegramBotClient bot,
        long chatId,
        Message message,
        CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        string path;
        int count;
        bool shouldFinish;

        try
        {
            path = await DownloadAndSavePhotoAsync(bot, message, cancellationToken, "news");
        }
        catch (InvalidOperationException ex)
        {
            await bot.SendMessage(chatId, $"⚠️ {ex.Message}", cancellationToken: cancellationToken);
            return;
        }

        lock (session)
        {
            if (session.State is not (TelegramBotState.WaitingForNewsImages or TelegramBotState.WaitingForNewNewsImages))
                return;

            if (session.DraftImagePaths.Count >= News.MaxImages)
            {
                shouldFinish = true;
                count = session.DraftImagePaths.Count;
            }
            else
            {
                session.DraftImagePaths.Add(path);
                count = session.DraftImagePaths.Count;
                shouldFinish = count >= News.MaxImages;
            }
        }

        if (shouldFinish)
        {
            await CompleteNewsImagesAsync(bot, chatId, cancellationToken);
            return;
        }

        await bot.SendMessage(
            chatId,
            $"Фото {count}/{News.MaxImages} сохранено. Ещё фото или «Готово».",
            replyMarkup: TelegramKeyboards.NewsPhotoStep(),
            cancellationToken: cancellationToken);
    }

    private async Task CompleteNewsImagesAsync(ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        List<string> paths;
        lock (session)
        {
            paths = session.DraftImagePaths.Take(News.MaxImages).ToList();
        }

        if (session.State == TelegramBotState.WaitingForNewsImages)
        {
            await CompleteAddNewsAsync(bot, chatId, paths, cancellationToken);
            return;
        }

        if (session.State == TelegramBotState.WaitingForNewNewsImages)
            await CompleteEditNewsImagesAsync(bot, chatId, paths, cancellationToken);
    }

    private async Task CompleteAddNewsAsync(
        ITelegramBotClient bot,
        long chatId,
        IReadOnlyList<string> imagePaths,
        CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);

        if (string.IsNullOrWhiteSpace(session.DraftTitle) ||
            string.IsNullOrWhiteSpace(session.DraftDescription) ||
            session.DraftEventDate is null)
        {
            session.Reset();
            await bot.SendMessage(chatId, "⚠️ Данные мастера утеряны. Начните заново.", cancellationToken: cancellationToken);
            await StartAddNewsWizardAsync(bot, chatId, cancellationToken);
            return;
        }

        var entity = new News
        {
            Title = session.DraftTitle,
            Summary = session.DraftDescription,
            PublishedAt = session.DraftEventDate.Value.Date
        };
        entity.SetImageTokens(imagePaths);

        await news.CreateAsync(entity, cancellationToken);
        session.Reset();
        await bot.SendMessage(chatId, "✅ Новость опубликована!", cancellationToken: cancellationToken);
        await SendNewsListAsync(bot, chatId, cancellationToken);
    }

    private async Task CompleteEditNewsImagesAsync(
        ITelegramBotClient bot,
        long chatId,
        IReadOnlyList<string> imagePaths,
        CancellationToken cancellationToken)
    {
        var session = stateService.GetOrCreate(chatId);
        if (session.NewsId is null)
            return;

        var entity = await news.GetByIdAsync(session.NewsId.Value, cancellationToken);
        if (entity is null)
            return;

        await DeleteUploadedNewsImagesAsync(entity, cancellationToken);
        entity.SetImageTokens(imagePaths);
        await news.UpdateAsync(entity, cancellationToken);

        var newsId = entity.Id;
        session.State = TelegramBotState.None;
        session.DraftImagePaths = [];

        await bot.SendMessage(chatId, "✅ Фото обновлены.", cancellationToken: cancellationToken);
        await SendNewsDetailsAsync(bot, chatId, newsId, cancellationToken);
    }

    private async Task DeleteUploadedNewsImagesAsync(News entity, CancellationToken cancellationToken)
    {
        foreach (var token in entity.GetImageTokens())
        {
            if (!token.StartsWith(News.TonePrefix, StringComparison.OrdinalIgnoreCase))
                await images.DeleteIfUploadedAsync(token, cancellationToken);
        }
    }

    private async Task<bool> EnsureNewsExists(
        ITelegramBotClient bot,
        long chatId,
        int newsId,
        CancellationToken cancellationToken)
    {
        if (await news.GetByIdAsync(newsId, cancellationToken) is not null)
            return true;

        await bot.SendMessage(chatId, "Новость не найдена.", cancellationToken: cancellationToken);
        await SendNewsListAsync(bot, chatId, cancellationToken);
        return false;
    }
}
