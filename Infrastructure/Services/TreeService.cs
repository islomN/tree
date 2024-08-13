using Domain.Models;
using Infrastructure.DataAccess;
using Infrastructure.Exceptions;

namespace Infrastructure.Services;

internal sealed class TreeService(
    ITreeDataService treeDataService)
    : ITreeService
{
    public async Task<Tree> Get(
        string treeName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(treeName))
        {
            throw new SecureException("Name is required");
        }
        
        if (await treeDataService.Exists(treeName, null, null, cancellationToken))
        {
            return await treeDataService.GetWithChildren(treeName, null, null, cancellationToken);
        }

        var tree = await treeDataService.Create(treeName, null, null, cancellationToken);

        return new Tree(
            tree.Id,
            tree.Name,
            Children: Array.Empty<Tree>());
    }
    
    public async Task Create(
        CreateTreeNodeRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new SecureException("Request model is empty");
        }

        if (string.IsNullOrWhiteSpace(request.TreeName))
        {
            throw new SecureException("Name is required");
        }

        if (string.IsNullOrWhiteSpace(request.NodeName))
        {
            throw new SecureException("Name is required");
        }

        if (request.ParentNodeId <= 0)
        {
            throw new SecureException("ParentNodeId is invalid");
        }

        var tree = await treeDataService.Get(
            request.TreeName,
            null,
            null,
            cancellationToken);
        if (tree is null)
        {
            throw new SecureException("Requested node was found, but it doesn't belong your tree");
        }

        if (tree.Id != request.ParentNodeId)
        {
            var treeNode = await treeDataService.Get(
                request.ParentNodeId,
                tree.Id,
                cancellationToken);

            if (treeNode is null)
            {
                throw new SecureException("Requested node was found, but it doesn't belong your tree");
            }
        }

        if (await treeDataService.ExistsName(
                request.NodeName,
                null,
                tree.Id,
                request.ParentNodeId,
                cancellationToken))
        {
            throw new SecureException("Duplicate name");
        }
        
        await treeDataService.Create(
            request.NodeName,
            tree.Id,
            request.ParentNodeId,
            cancellationToken);
    }

    public async Task Rename(
        RenameTreeRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new SecureException("Request model is empty");
        }
        
        if (string.IsNullOrWhiteSpace(request.TreeName))
        {
            throw new SecureException("Name is required");
        }
        
        if (string.IsNullOrWhiteSpace(request.NewNodeName))
        {
            throw new SecureException("NewNodeName is required");
        }

        if (request.NodeId <= 0)
        {
            throw new SecureException("NodeId is invalid");
        }

        var tree = await treeDataService.Get(
            request.TreeName,
            null,
            null,
            cancellationToken);
        
        if (tree is null)
        {
            throw new SecureException("Requested node was found, but it doesn't belong your tree");
        }

        if (tree.Id == request.NodeId)
        {
            if (await treeDataService.ExistsName(
                    request.NewNodeName,
                    tree.Id,
                    null,
                    null,
                    cancellationToken))
            {
                throw new SecureException("Duplicate name");
            }
        }
        else
        {
            var treeNode = await treeDataService.Get(request.NodeId, tree.Id, cancellationToken);

            if (treeNode is null)
            {
                throw new SecureException("Requested node was found, but it doesn't belong your tree");
            }

            if (await treeDataService.ExistsName(
                    request.NewNodeName,
                    request.NodeId,
                    tree.Id,
                    treeNode.ParentId,
                    cancellationToken))
            {
                throw new SecureException("Duplicate name");
            }
        }
        
        await treeDataService.Rename(tree.Id, request.NewNodeName, cancellationToken);
    }

    public async Task Delete(
        string treeName,
        int id,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(treeName))
        {
            throw new SecureException("Name is required");
        }

        if (id <= 0)
        {
            throw new SecureException("NodeId is invalid");
        }
        
        var tree = await treeDataService.Get(treeName, null, null, cancellationToken);
        
        if (tree is null)
        {
            throw new SecureException(
                "Requested node was found, but it doesn't belong your tree");
        }

        if (tree.Id != id)
        {
            var treeNode = await treeDataService.Get(id, tree.Id, cancellationToken);

            if (treeNode is null)
            {
                throw new SecureException(
                    "Requested node was found, but it doesn't belong your tree");
            }
        }

        if (await treeDataService.ExistsChildren(id, cancellationToken))
        {
            throw new SecureException("You have to delete all children nodes first");
        }

        await treeDataService.Delete(id, cancellationToken);
    }
}