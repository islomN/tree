using Database;
using Database.Tables;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tree = Domain.Models.Tree;

namespace Infrastructure.DataAccess;

internal sealed class TreeDataService(IOptions<EntityContextOptions> options)
    : ITreeDataService 
{
    public async Task<TreeModel> Get(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = await context.Trees
            .FirstOrDefaultAsync(
                i => i.Name == name && i.FirstParentId == firstParentId && i.ParentId == parentId,
                cancellationToken);

        return item is null ? null! : new TreeModel(item.Id, item.Name, item.ParentId);
    }
    
    public async Task<TreeModel> Get(
        int id,
        int firstParentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);

        var item = await context.Trees
            .FirstOrDefaultAsync(
                i => i.Id == id && i.FirstParentId == firstParentId,
                cancellationToken);

        return item is null
            ? null!
            : new TreeModel(
                item.Id,
                item.Name,
                item.ParentId);
    }

    public async Task<Tree> GetWithChildren(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options, true);
        
        var item = await context.Trees
            .FirstOrDefaultAsync(
                i => i.Name == name && i.FirstParentId == firstParentId && i.ParentId == parentId,
                cancellationToken);

        var trees =  new Tree(
            item!.Id,
            item.Name,
            (item.Children ?? Array.Empty<Database.Tables.Tree>()).Select(ConvertToModel).ToList());
        
        return trees;
    }
    
    public async Task<bool> Exists(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        return await context.Trees
            .AnyAsync(
                i => i.Name == name
                     && i.FirstParentId == firstParentId
                     && i.ParentId == parentId,
                cancellationToken);
    }

    public async Task<bool> ExistsChildren(
        int id,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        return await context.Trees
            .AnyAsync(
                i => i.ParentId == id,
                cancellationToken);
    }

    public async Task<bool> ExistsName(
        string name,
        int? id,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        return await context.Trees
            .AnyAsync(
                i => i.FirstParentId == firstParentId
                     && i.ParentId == parentId
                     && i.Id != id
                     && i.Name == name,
                cancellationToken);
    }

    public async Task<TreeModel> Create(string name, CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = new Database.Tables.Tree(default, name, null, null);
        
        context.Trees.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        
        return new TreeModel(item.Id, item.Name, item.ParentId);
    }
    
    public async Task<TreeModel> Create(
        string name,
        int? firstParentId,
        int? parentId,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = new Database.Tables.Tree(
            default,
            name,
            firstParentId,
            parentId);
        
        context.Trees.Add(item);
        await context.SaveChangesAsync(cancellationToken);
        
        return new TreeModel(item.Id, item.Name, item.ParentId);
    }

    public async Task Rename(
        int id,
        string name,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = await context.Trees
            .AsNoTracking()
            .FirstAsync(i => i.Id == id, cancellationToken);
        
        item = item with
        {
            Name = name,
            UpdatedAt = DateTime.UtcNow
        };

        context.Trees.Update(item);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await using var context = new EntityContext(options);
        
        var item = await context.Trees
            .FirstAsync(
                i => i.Id == id,
                cancellationToken);
        
        context.Trees.Remove(item);
        
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private Tree ConvertToModel(Database.Tables.Tree node)
    {
        if (node is null)
            return null!;

        return new Tree(
            node.Id,
            node.Name,
            (node.Children ?? Array.Empty<Database.Tables.Tree>()).Select(ConvertToModel).ToList());
    }
}