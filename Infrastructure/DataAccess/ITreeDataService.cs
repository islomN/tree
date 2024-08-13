using Domain.Models;

namespace Infrastructure.DataAccess;

internal interface ITreeDataService
{
    Task<TreeModel> Get(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken);
    
    Task<TreeModel> Get(
        int id,        
        int firstParentId,
        CancellationToken cancellationToken);
    
    Task<bool> Exists(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken);

    Task<bool> ExistsChildren(
        int id,
        CancellationToken cancellationToken);
    
    Task<bool> ExistsName(
        string name,
        int? id,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken);
    
    Task<Tree> GetWithChildren(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken);
    
    Task<TreeModel> Create(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken);
    
    Task Rename(
        int id,
        string name,
        CancellationToken cancellationToken);
    
    Task Delete(
        int id,
        CancellationToken cancellationToken);
}