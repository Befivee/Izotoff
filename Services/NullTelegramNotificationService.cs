using Izotoff.Models;

namespace Izotoff.Services;

public class NullTelegramNotificationService : ITelegramNotificationService
{
    public Task<bool> NotifyNewBookingAsync(Booking booking, CancellationToken cancellationToken = default) =>
        Task.FromResult(true);
}
