using System.ComponentModel.DataAnnotations;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api.[controller].[action]")]
public class TreeNodeController(ITreeService treeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateTreeNodeRequest request)
    {
        await treeService.Create(request, HttpContext.RequestAborted);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Rename([FromBody]RenameTreeRequest request)
    {
        await treeService.Rename(request, HttpContext.RequestAborted);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([Required]string treeName, [Required]int nodeId)
    {
        await treeService.Delete(treeName, nodeId, HttpContext.RequestAborted);
        return Ok();
    }
}