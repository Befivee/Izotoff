namespace Izotoff.Services.Telegram;

public enum TelegramBotState
{
    None,
    WaitingForEventTitle,
    WaitingForEventDescription,
    WaitingForEventDate,
    WaitingForEventImage,
    WaitingForNewTitle,
    WaitingForNewDescription,
    WaitingForNewImage,
    WaitingForNewsTitle,
    WaitingForNewsDescription,
    WaitingForNewsDate,
    WaitingForNewsImages,
    WaitingForNewNewsTitle,
    WaitingForNewNewsDescription,
    WaitingForNewNewsDate,
    WaitingForNewNewsImages
}
