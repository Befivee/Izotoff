using Izotoff.Models;

namespace Izotoff.Services;

public interface ITelegramNotificationService
{
    /// <returns>true, если уведомление отправлено или Telegram отключён.</returns>
    Task<bool> NotifyNewBookingAsync(Booking booking, CancellationToken cancellationToken = default);
}
