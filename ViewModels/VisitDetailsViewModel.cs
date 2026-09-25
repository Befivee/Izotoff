using Izotoff.Models;

namespace Izotoff.ViewModels;

public class VisitDetailsViewModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? ScheduleLabel { get; init; }
    public DateTime? EventDate { get; init; }
    public string? ImagePath { get; init; }
    public bool IsPinned { get; init; }
    public bool IsCollaboration { get; init; }
    public string BookingTourKind { get; init; } = "self";

    public static VisitDetailsViewModel FromPinned() => new()
    {
        Title = PinnedVisit.Title,
        Description = PinnedVisit.Description,
        ScheduleLabel = PinnedVisit.ScheduleLabel,
        IsPinned = true,
        BookingTourKind = "guided"
    };

    public static VisitDetailsViewModel FromEvent(Event entity) => new()
    {
        Title = entity.Title,
        Description = entity.Description,
        EventDate = entity.EventDate,
        ImagePath = entity.DisplayImagePath,
        IsPinned = false,
        IsCollaboration = entity.IsCollaboration,
        BookingTourKind = "self"
    };
}
