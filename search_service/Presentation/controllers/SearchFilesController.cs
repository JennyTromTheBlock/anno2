namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchFilesController : ControllerBase
    {
        public SearchFilesController()
        {
        }

        [HttpGet]
        public IActionResult Search([FromQuery] string query)
        {
            // TODO: Call application service to search files
            return Ok($"Searched for '{query}'.");
        }
    }
}
