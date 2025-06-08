namespace Presentation.Controllers;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PdfStorageController : ControllerBase
{
    private readonly IPdfStorageService _pdfStorageService;

    public PdfStorageController(IPdfStorageService pdfStorageService)
    {
        _pdfStorageService = pdfStorageService;
    }

    /// <summary>
    /// Hent en enkelt PDF ud fra en relativ sti.
    /// </summary>
    /// <param name="relativePath">Den relative sti til PDF-filen</param>
    [HttpGet("{*relativePath}")]
    public async Task<IActionResult> GetPdf(string relativePath)
    {
        try
        {
            var fileBytes = await _pdfStorageService.GetPdfAsync(relativePath);
            return File(fileBytes, "application/pdf", Path.GetFileName(relativePath));
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "PDF ikke fundet" });
        }
    }

    /// <summary>
    /// Hent flere PDF-filer ud fra en liste af relative stier.
    /// </summary>
    /// <param name="paths">Liste af stier</param>
    [HttpPost("batch")]
    public async Task<IActionResult> GetMultiplePdfs([FromBody] List<string> paths)
    {
        var files = await _pdfStorageService.GetMultiplePdfsAsync(paths);

        if (files.Count == 0)
            return NotFound(new { message = "Ingen PDF-filer blev fundet." });

        return Ok(files); // alternativ: returnér som base64 eller pak dem i ZIP
    }
}
