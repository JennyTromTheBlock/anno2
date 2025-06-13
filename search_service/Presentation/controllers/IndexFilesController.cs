using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndexFilesController : ControllerBase
    {
        public IndexFilesController()
        {
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
    }
}