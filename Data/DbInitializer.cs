using Microsoft.EntityFrameworkCore;

namespace Izotoff.Data;

public static class DbInitializer
{
    /// <summary>
    /// Применяет миграции EF Core при старте приложения.
    /// Демо-данные не добавляются — контент создаётся через админку и ботов.
    /// </summary>
    public static async Task InitializeAsync(ApplicationDbContext context) =>
        await context.Database.MigrateAsync();
}
