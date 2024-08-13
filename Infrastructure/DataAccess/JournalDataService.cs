using Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.DataAccess;

internal sealed class JournalDataService(IOptions<EntityContextOptions> options)
    : IJournalDataService
{
    public async Task<JournalListResponse> Select(
        int skip,
        int take,
        DateTime startDate,
        DateTime endDate,
        string search,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var query = context.Journals
            .Where(i => startDate <= i.CreatedAt)
            .Where(i => endDate >= i.CreatedAt);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(i => i.Text.Contains(search));
        }
        
        var count = query.Count();
        var items = await query
            .Select(i => new JournalItemModel(i.Id, i.EventId, i.CreatedAt))
            .Skip(skip)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return new JournalListResponse(skip, count, items);
    }

    public async Task<JournalResponse> Get(
        int id,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = await context.Journals
            .FirstOrDefaultAsync(
                i => i.Id == id,
                cancellationToken);

        if (item is not null)
        {
            return item is null
                ? null
                : new JournalResponse(
                    item.Id,
                    item.EventId,
                    item.Text,
                    item.CreatedAt);
        }

        throw new Exception();
    }
}