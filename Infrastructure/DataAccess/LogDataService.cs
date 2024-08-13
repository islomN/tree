using Database;
using Database.Tables;
using Microsoft.Extensions.Options;

namespace Infrastructure.DataAccess;

internal sealed class LogDataService(IOptions<EntityContextOptions> options)
    : ILogDataService
{
    public async Task Save(long eventId, string log, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options.Value.ConnectionString);
        context.Journals.Add(new Journal(default, eventId, log));
        await context.SaveChangesAsync(cancellationToken);
    }
}