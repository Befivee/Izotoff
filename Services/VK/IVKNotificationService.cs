using Izotoff.Models;

namespace Izotoff.Services.VK;

public interface IVKNotificationService
{
    /// <returns>true, если уведомление отправлено или VK отключён.</returns>
    Task<bool> NotifyNewBookingAsync(Booking booking, CancellationToken cancellationToken = default);
}
