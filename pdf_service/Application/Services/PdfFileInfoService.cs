using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using PdfSharpCore.Pdf.IO;

namespace Application.Services;
public class PdfFileInfoService : IPdfFileInfoService
{
    private readonly IPdfFileInfoRepository _repository;

    public PdfFileInfoService(IPdfFileInfoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PdfFile?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<PdfFile>> GetByIdsAsync(List<int> ids)
    {
        return await _repository.GetByIdsAsync(ids);
    }

    public async Task<PdfFile> CreateAsync(PdfFileCreateDto dto, int userId,  string path)
    {
        // get meta data
        var pageCount = await GetPageNumbersInPdf(dto.File);

        // save the fileinfo in db
        var pdfFile = new PdfFile
        {
            FileName = dto.FileName,
            AuthorId = userId,
            Path = path,
            CreatedAt = DateTime.UtcNow,
            Pages = pageCount
        };
        return await _repository.CreateAsync(pdfFile);
    }

    public async Task UpdateAsync(PdfFile pdfFile)
    {
        await _repository.UpdateAsync(pdfFile);
    }

    public async Task SoftDeleteAsync(int id)
    {
        await _repository.SoftDeleteAsync(id);
    }

    private async Task<int> GetPageNumbersInPdf(IFormFile file)
    {
        int pageCount = 0;

        await using var stream = file.OpenReadStream();
        var document = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
        pageCount = document.PageCount;

        return pageCount;
    }
}
