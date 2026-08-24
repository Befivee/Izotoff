namespace Izotoff.Services;

public interface IBackupService
{
    Task<string> ExportBookingsAndEventsAsync(CancellationToken cancellationToken = default);
}
