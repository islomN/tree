namespace Infrastructure.Services;

public interface ILogService
{
    Task Save(long eventId, string log, CancellationToken cancellationToken);
}