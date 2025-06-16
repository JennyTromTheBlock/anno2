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
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] CaseSearchQueryDto dto)
    {
        var results = await _elasticService.SearchAsync(dto.Query, dto.DocumentId.ToString());
        return Ok(results);
    }
}
