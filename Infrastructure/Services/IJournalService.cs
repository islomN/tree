using Domain.Models;

namespace Infrastructure.Services;

public interface IJournalService
{
    Task<JournalListResponse> GetRange(
        int skip,
        int take,
        DateTime startDate,
        DateTime endDate,
        string search,
        CancellationToken cancellationToken);

    Task<JournalResponse> GetSingle(
        int id,
        CancellationToken cancellationToken);
}