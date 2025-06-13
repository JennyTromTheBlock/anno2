using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/pdf")]
public class PdfController : ControllerBase
{
    private readonly ElasticSearchService _elasticService;

    public PdfController(ElasticSearchService elasticService)
    {
        _elasticService = elasticService;
    }

    [HttpPost("index")]
    public async Task<IActionResult> IndexPdf(IFormFile file, [FromQuery] string documentId)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var pdfBytes = ms.ToArray();

        await _elasticService.IndexPdfAsync(pdfBytes, documentId);
        return Ok("PDF indekseret");
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string word, [FromQuery] string documentId)
    {
        var result = await _elasticService.SearchAsync(word, documentId);
        return Ok(result);
    }
}
