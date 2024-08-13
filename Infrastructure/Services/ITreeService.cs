using Domain.Models;

namespace Infrastructure.Services;

public interface ITreeService
{
    Task<Tree> Get(
        string treeName,
        CancellationToken cancellationToken);
    
    Task Create(
        CreateTreeNodeRequest request,
        CancellationToken cancellationToken);

    Task Rename(
        RenameTreeRequest request,
        CancellationToken cancellationToken);

    Task Delete(
        string treeName,
        int id,
        CancellationToken cancellationToken);
}