using Infrastructure.DataAccess;

namespace Infrastructure.Services;

internal sealed class LogService(ILogDataService dataService) : ILogService
{
    public Task Save(long eventId, string log, CancellationToken cancellationToken)
    {
        return dataService.Save(eventId, log, cancellationToken);
    }
}