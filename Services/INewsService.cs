using Izotoff.Models;

namespace Izotoff.Services;

public interface INewsService
{
    Task<IReadOnlyList<News>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<News>> GetLatestAsync(int count, CancellationToken cancellationToken = default);
    Task<News?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<News> CreateAsync(News entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(News entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
