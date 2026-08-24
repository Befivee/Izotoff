using Izotoff.Models;

namespace Izotoff.Services;

public interface IBookingNotificationService
{
    /// <summary>Ставит заявку в очередь уведомлений (Telegram + VK), не блокируя HTTP-ответ сайта.</summary>
    void ScheduleNewBookingNotification(Booking booking);
}
