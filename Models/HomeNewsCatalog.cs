namespace Izotoff.Models;

public sealed class NewsMediaSlide
{
    public string? ImageUrl { get; init; }
    public string? ToneClass { get; init; }

    public static NewsMediaSlide FromToken(string token)
    {
        if (token.StartsWith(News.TonePrefix, StringComparison.OrdinalIgnoreCase))
            return new NewsMediaSlide { ToneClass = "tone-" + token[News.TonePrefix.Length..] };

        return new NewsMediaSlide { ImageUrl = token };
    }
}

public sealed class HomeNewsItem
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required DateTime PublishedAt { get; init; }
    public IReadOnlyList<NewsMediaSlide> Slides { get; init; } = [];
}

public static class HomeNewsCatalog
{
    public static IReadOnlyList<News> SeedItems { get; } =
    [
        new News
        {
            Title = "Заложен первый виноградник Калининградской области",
            Summary =
                "На территории эко-фермы IZOTOFF началась закладка лоз на площади 5 гектаров. " +
                "Выбраны сорта Пино нуар и Пино гри — они станут основой будущих вин программы хозяйства.",
            PublishedAt = new DateTime(2025, 4, 12),
            ImagePaths = "tone:forest;tone:vine;tone:dusk"
        },
        new News
        {
            Title = "Открыт дегустационный зал с видом на лозы",
            Summary =
                "Гости фермы смогут пробовать сыры «Изотов-Чиз» и сезонные напитки в новом зале " +
                "с панорамным остеклением. Пространство готовят к винным и гастрономическим вечерам.",
            PublishedAt = new DateTime(2025, 8, 3),
            ImagePaths = "tone:wine;tone:sage"
        },
        new News
        {
            Title = "Сезон яблочного сидра стартовал на ферме",
            Summary =
                "Осенняя ферментация — продолжение семейной истории IZOTOFF: после молочного сезона " +
                "команда хозяйства переключается на яблочный урожай и эксперименты с игристыми напитками.",
            PublishedAt = new DateTime(2025, 10, 18),
            ImagePaths = "tone:amber;tone:forest;tone:clay"
        }
    ];
}
