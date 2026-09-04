using Microsoft.EntityFrameworkCore;
using Izotoff.Models;

namespace Izotoff.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();
        await SeedNewsAsync(context);
    }

    private static async Task SeedNewsAsync(ApplicationDbContext context)
    {
        if (await context.News.AnyAsync())
            return;

        var now = DateTime.UtcNow;
        foreach (var item in HomeNewsCatalog.SeedItems)
        {
            item.CreatedAt = now;
            item.UpdatedAt = now;
            context.News.Add(item);
        }

        await context.SaveChangesAsync();
    }
}
