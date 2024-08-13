using System.ComponentModel.DataAnnotations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api.[controller].[action]")]
public class TreeController(ITreeService treeService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Get([Required]string treeName)
    {
        return Ok(await treeService.Get(treeName, HttpContext.RequestAborted));
    }
}