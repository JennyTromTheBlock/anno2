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

        [HttpPost]
        public IActionResult IndexFile([FromBody] string filePath)
        {
            // TODO: Call application service to index file
            return Ok($"File at '{filePath}' indexed.");
        }
    }
}