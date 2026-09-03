namespace Izotoff.Models;

/// <summary>Постоянный блок на странице «Посещение». Не хранится в БД и недоступен ботам.</summary>
public static class PinnedVisit
{
    public const string Title = "Посещение эко-фермы IZOTOFF";

    public const string Description =
        "Знакомство с семейной фермой: животные, сыроварня, история хозяйства. " +
        "Встреча с животными, дегустации, прогулка по территории. По предварительной записи.";

    public const string ScheduleLabel = "По записи";
}
