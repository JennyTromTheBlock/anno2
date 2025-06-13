using Application.Interfaces.Services;
using Application.Services;


namespace Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PdfFileInfoController : ControllerBase
{
    private readonly IPdfFileInfoService _service;
    private readonly IPdfStorageService _pdfStorageService;
    private readonly ElasticSearchService _searchService;


    public PdfFileInfoController(IPdfFileInfoService service, IPdfStorageService pdfStorageService, ElasticSearchService searchService)
    {
        _service = service;
        _pdfStorageService = pdfStorageService;
        _searchService = searchService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var pdfFile = await _service.GetByIdAsync(id);
        if (pdfFile == null)
            return NotFound();

        return Ok(pdfFile);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PdfFileCreateDto dto)
    {
        int userId = 1; // todo skal være fra auth 

        var path = await _pdfStorageService.SavePdfAsync(dto.File);
        var createdPdfFile = await _service.CreateAsync(dto, userId, path);

        var file = await _pdfStorageService.GetPdfAsync(createdPdfFile.Path);
        // todo skal kører ocr først 
        await _searchService.IndexPdfAsync(file, createdPdfFile.Id.ToString());

        return CreatedAtAction(nameof(Get), new { id = createdPdfFile.Id }, createdPdfFile);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PdfFile pdfFile)
    {
        if (id != pdfFile.Id)
            return BadRequest("Id mismatch");

        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _service.UpdateAsync(pdfFile);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _service.SoftDeleteAsync(id);
        return NoContent();
    }
}
