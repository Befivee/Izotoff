namespace Izotoff.Services.VK;

public enum VKBotState
{
    None,
    WaitingForEventTitle,
    WaitingForEventDescription,
    WaitingForEventDate,
    WaitingForEventImage,
    WaitingForNewTitle,
    WaitingForNewDescription,
    WaitingForNewImage
}
