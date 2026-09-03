using Izotoff.Models;

namespace Izotoff.ViewModels;

public class VisitIndexViewModel
{
    public IReadOnlyList<Event> Visits { get; init; } = [];
}
