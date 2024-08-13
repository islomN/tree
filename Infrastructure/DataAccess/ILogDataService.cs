namespace Infrastructure.DataAccess;

internal interface ILogDataService
{
    Task Save(long eventId, string log, CancellationToken cancellationToken);
}