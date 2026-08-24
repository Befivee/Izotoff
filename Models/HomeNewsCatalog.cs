namespace Izotoff.Models;

public sealed class HomeNewsItem
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required DateTime PublishedAt { get; init; }
    public required IReadOnlyList<string> ImageTones { get; init; }
}

public static class HomeNewsCatalog
{
    public static IReadOnlyList<HomeNewsItem> Featured { get; } =
    [
        new HomeNewsItem
        {
            Title = "Заложен первый виноградник Калининградской области",
            Summary =
                "На территории эко-фермы IZOTOFF началась закладка лоз на площади 5 гектаров. " +
                "Выбраны сорта Пино нуар и Пино гри — они станут основой будущих вин программы хозяйства.",
            PublishedAt = new DateTime(2025, 4, 12),
            ImageTones = ["tone-forest", "tone-vine", "tone-dusk"]
        },
        new HomeNewsItem
        {
            Title = "Открыт дегустационный зал с видом на лозы",
            Summary =
                "Гости фермы смогут пробовать сыры «Изотов-Чиз» и сезонные напитки в новом зале " +
                "с панорамным остеклением. Пространство готовят к винным и гастрономическим вечерам.",
            PublishedAt = new DateTime(2025, 8, 3),
            ImageTones = ["tone-wine", "tone-sage"]
        },
        new HomeNewsItem
        {
            Title = "Сезон яблочного сидра стартовал на ферме",
            Summary =
                "Осенняя ферментация — продолжение семейной истории IZOTOFF: после молочного сезона " +
                "команда хозяйства переключается на яблочный урожай и эксперименты с игристыми напитками.",
            PublishedAt = new DateTime(2025, 10, 18),
            ImageTones = ["tone-amber", "tone-forest", "tone-clay"]
        }
    ];
}
