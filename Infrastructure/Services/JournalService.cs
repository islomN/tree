using Domain.Models;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;

namespace Infrastructure.Services;

internal sealed class JournalService(IJournalDataService dataService) : IJournalService
{
    public Task<JournalListResponse> GetRange(
        int skip,
        int take,
        DateTime startDate,
        DateTime endDate,
        string search,
        CancellationToken cancellationToken)
    {
        if (skip < 0)
        {
            throw new SecureException("Skip must be greater than 0 or equal 0");
        }

        if (take <= 0)
        {
            throw new SecureException("Take must be greater than 0");
        }

        if (startDate > endDate)
        {
            throw new SecureException("EndDate must be grater than StartDate");
        }

        return dataService.Select(skip, take, startDate, endDate, search, cancellationToken);
    }

    public Task<JournalResponse> GetSingle(
        int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            throw new SecureException("Id is invalid");
        }
        
        return dataService.Get(id, cancellationToken);
    }
}