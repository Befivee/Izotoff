namespace Izotoff.Models;

public sealed class ExcursionTypeInfo
{
    public ExcursionKind Kind { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Duration { get; init; } = string.Empty;
    public decimal RegularPrice { get; init; }
    public decimal ConcessionPrice { get; init; } = ExcursionCatalog.ConcessionPrice;
    public string ImagePath { get; init; } = Excursion.DefaultImagePath;
    public bool RequiresTimeSlot { get; init; }

    public string MediaPlaceholderClass =>
        Kind == ExcursionKind.Guided
            ? "excursion-type-block__media--guided"
            : "excursion-type-block__media--vineyard";

    public int Id => (int)Kind;
    public string KindKey => Kind == ExcursionKind.Guided ? "guided" : "self";
    public string FormLabel => Kind == ExcursionKind.Guided ? "На ферму" : "На виноградник";
    public string DisplayPrice => $"от {RegularPrice:0} ₽";
    public string PriceDetail => $"{RegularPrice:0} ₽ с человека";
    public string FormPriceLabel => $"{RegularPrice:0} ₽";
}

public static class ExcursionCatalog
{
    public const decimal ConcessionPrice = 500;
    public const decimal GuidedRegularPrice = 500;
    public const decimal SelfGuidedRegularPrice = 1000;

    public static readonly ExcursionTypeInfo Guided = new()
    {
        Kind = ExcursionKind.Guided,
        Title = "Экскурсия на ферму IZOTOFF",
        Description =
            "Знакомство с семейной фермой: животные, сыроварня, история хозяйства. " +
            "Встреча с животными, дегустации, экскурсии на ферме.",
        Duration = "1–3 часа",
        RegularPrice = GuidedRegularPrice,
        ConcessionPrice = GuidedRegularPrice,
        RequiresTimeSlot = true
    };

    public static readonly ExcursionTypeInfo SelfGuided = new()
    {
        Kind = ExcursionKind.SelfGuided,
        Title = "Гастрономическое путешествие на виноградник",
        Description =
            "Прогулка среди рядов молодого виноградника на площади 5 гектаров. " +
            "Дегустации, знакомство с сортами Пино нуар и Пино гри и фермерскими сырами.",
        Duration = "1–3 часа",
        RegularPrice = SelfGuidedRegularPrice,
        ConcessionPrice = SelfGuidedRegularPrice,
        RequiresTimeSlot = true
    };

    public static IReadOnlyList<ExcursionTypeInfo> All { get; } = [Guided, SelfGuided];

    public static string[] GuidedTimeSlots { get; } =
        ["10:00", "11:00", "12:00", "12:30", "13:00", "14:00", "15:00", "16:00", "17:00"];

    public static bool TryGetById(int? id, out ExcursionTypeInfo info)
    {
        info = All.FirstOrDefault(item => item.Id == id)!;
        return info is not null;
    }

    public static ExcursionTypeInfo Get(ExcursionKind kind) =>
        kind == ExcursionKind.Guided ? Guided : SelfGuided;
}
