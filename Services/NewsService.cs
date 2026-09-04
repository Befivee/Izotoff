using Microsoft.EntityFrameworkCore;
using Izotoff.Data;
using Izotoff.Models;

namespace Izotoff.Services;

public class NewsService(ApplicationDbContext context) : INewsService
{
    public async Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await context.News
            .OrderByDescending(item => item.PublishedAt)
            .ThenByDescending(item => item.Id)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<News>> GetLatestAsync(int count, CancellationToken cancellationToken = default) =>
        await context.News
            .OrderByDescending(item => item.PublishedAt)
            .ThenByDescending(item => item.Id)
            .Take(count)
            .ToListAsync(cancellationToken);

    public async Task<News?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await context.News.FindAsync([id], cancellationToken);

    public async Task<News> CreateAsync(News entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
        context.News.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(News entity, CancellationToken cancellationToken = default)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        context.News.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await context.News.FindAsync([id], cancellationToken);
        if (entity is null)
            return;

        context.News.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
