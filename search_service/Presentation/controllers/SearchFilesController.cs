using Application.Domains.DTOs;
using Application.Interfaces.Services;

namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SearchFilesController : ControllerBase
{
    private readonly IElasticSearchService _elasticService;

    // Inject ElasticSearch service i constructor
    public SearchFilesController(IElasticSearchService elasticService)
    {
        _elasticService = elasticService;
    }
    
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] CaseSearchQueryDto dto)
    {
        try
        {
            var results = await _elasticService.SearchWordPositionsAsync(dto);
            return Ok(results);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Fejl under søgning: {ex.Message}");
        }
    }
}
