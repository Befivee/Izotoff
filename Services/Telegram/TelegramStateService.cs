using System.Collections.Concurrent;

namespace Izotoff.Services.Telegram;

public class TelegramStateService
{
    private readonly ConcurrentDictionary<long, TelegramUserSession> _sessions = new();

    public TelegramUserSession GetOrCreate(long chatId) =>
        _sessions.GetOrAdd(chatId, _ => new TelegramUserSession());

    public void Clear(long chatId) => _sessions.TryRemove(chatId, out _);
}
