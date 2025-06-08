namespace Presentation.Controllers;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PdfStorageController : ControllerBase
{
    private readonly IPdfStorageService _pdfStorageService;
    private readonly IPdfFileInfoService _pdfFileInfoService;

    public PdfStorageController(IPdfStorageService pdfStorageService, IPdfFileInfoService pdfFileInfoService)
    {
        _pdfStorageService = pdfStorageService;
        _pdfFileInfoService = pdfFileInfoService;
    }

    [HttpGet("{pdfId}")]
    public async Task<IActionResult> GetPdf(int pdfId)
    {
        try
        {
            var pdfInfo = await _pdfFileInfoService.GetByIdAsync(pdfId);
            if (pdfInfo == null)
                return NotFound(new { message = "Ingen PDF-info fundet for ID'et." });

            var pdfFile = await _pdfStorageService.GetPdfAsync(pdfInfo.Path);

            return File(pdfFile, "application/pdf", $"{pdfInfo.FileName ?? "download"}.pdf");
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "PDF-fil ikke fundet." });
        }
    }
    
    
    [HttpPost("batch")]
    public async Task<IActionResult> GetMultiplePdfs([FromBody] List<int> idsToGet)
    {
        var pdfInfoList = await _pdfFileInfoService.GetByIdsAsync(idsToGet);

        if (pdfInfoList == null || !pdfInfoList.Any())
            return NotFound(new { message = "Ingen PDF-info fundet for de angivne ID'er." });

        var result = new List<object>();

        foreach (var pdfInfo in pdfInfoList)
        {
            if (string.IsNullOrWhiteSpace(pdfInfo.Path))
                continue;

            try
            {
                var pdfBytes = await _pdfStorageService.GetPdfAsync(pdfInfo.Path);

                result.Add(new
                {
                    Id = pdfInfo.Id,
                    FileName = pdfInfo.FileName,
                    Path = pdfInfo.Path,
                    CreatedAt = pdfInfo.CreatedAt,
                    AuthorId = pdfInfo.AuthorId,
                    FileContent = Convert.ToBase64String(pdfBytes)
                });
            }
            catch (FileNotFoundException)
            {
                // valgfrit: log eller spring over
            }
        }

        if (!result.Any())
            return NotFound(new { message = "Ingen PDF-filer kunne hentes." });

        return Ok(result);
    }

}
