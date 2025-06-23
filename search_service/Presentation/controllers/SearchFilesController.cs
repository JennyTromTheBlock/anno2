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
    [HttpGet("searchWord")]
    public async Task<IActionResult> SearchWord([FromQuery] CaseSearchQueryDto dto)
    {
        try
        {
            var results = await _elasticService.GetPdfWithWordsAsync(dto);
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

    [HttpPost("init")]
    public async Task<IActionResult> initElasticShemes()
    {
        try
        {
            await _elasticService.InitElasticSearch();
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Fejl under søgning: {e.Message}");

        }
    }
}
