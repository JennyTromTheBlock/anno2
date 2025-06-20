using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndexFilesController : ControllerBase
    {
        private readonly IElasticSearchService _elasticService;

        // Inject ElasticSearch service i constructor
        public IndexFilesController(IElasticSearchService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpPost("index")]
        public async Task<IActionResult> IndexPdf(
            IFormFile file,
            [FromQuery] string documentId,
            [FromQuery] string? caseId = null,
            [FromQuery] string? attachmentId = null,
            [FromQuery] string? fileName = null)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Filen er tom eller mangler.");
            }

            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var pdfBytes = ms.ToArray();

                await _elasticService.IndexPdfAsync(pdfBytes, documentId, caseId, attachmentId, fileName);
                return Ok("PDF indekseret.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Ugyldig fil: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Filen indeholder ingen ord: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fejl under indeksering: {ex.Message}");
            }
        }
    }

}