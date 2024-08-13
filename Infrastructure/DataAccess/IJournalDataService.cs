using Domain.Models;

namespace Infrastructure.DataAccess;

internal interface IJournalDataService
{
    Task<JournalListResponse> Select(
        int skip,
        int take,
        DateTime startDate,
        DateTime endDate,
        string search,
        CancellationToken cancellationToken);

    Task<JournalResponse> Get(int id, CancellationToken cancellationToken);
}