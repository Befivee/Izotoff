using Izotoff.Models;

namespace Izotoff.ViewModels;

public class HomeIndexViewModel
{
    public IReadOnlyList<HomeNewsItem> FeaturedNews { get; init; } = [];

    public IReadOnlyList<Event> UpcomingEvents { get; init; } = [];
}
