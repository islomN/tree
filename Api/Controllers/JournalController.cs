using System.ComponentModel.DataAnnotations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api.[controller].[action]")]
public class JournalController(IJournalService journalService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRange(
        [Required]
        int skip,
        [Required]
        int take,
        [Required]
        DateTime startDate,
        [Required]
        DateTime endDate,
        string text)
    {
        return Ok(
            await journalService.GetRange(
                skip,
                take,
                startDate,
                endDate,
                text,
                HttpContext.RequestAborted));
    }

    [HttpGet]
    public async Task<IActionResult> GetSingle([Required]int id)
    {
        return Ok(await journalService.GetSingle(id, HttpContext.RequestAborted));
    }
}