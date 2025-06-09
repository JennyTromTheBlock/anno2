using Application.Domain.Messages;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using EasyNetQ;
using Microsoft.AspNetCore.Http;
using PdfSharpCore.Pdf.IO;
using RabbitMQ.Client;

namespace Application.Services;
public class PdfFileInfoService : IPdfFileInfoService
{
    private readonly IPdfFileInfoRepository _repository;
    private readonly IBus _bus;

    public PdfFileInfoService(IPdfFileInfoRepository repository, IBus bus)
    {
        _repository = repository;
        _bus = bus;

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
            AttId =dto.AttId,
            AuthorId = userId,
            Path = path,
            CreatedAt = DateTime.UtcNow,
            Pages = pageCount
        };
        var createdFileInfo = await _repository.CreateAsync(pdfFile);

        //updates system with att if set by user
        if (dto.AttId != 0)
        {
            await HandleNewFileEventAsync(createdFileInfo);
        }
        return createdFileInfo;
    }

    public async Task UpdateAsync(PdfFile pdfFile)
    {
        await _repository.UpdateAsync(pdfFile);
    }

    public async Task SoftDeleteAsync(int id)
    {
        await _repository.SoftDeleteAsync(id);
        await HandleDeletedFileEvent(id);
    }

    private async Task<int> GetPageNumbersInPdf(IFormFile file)
    {
        int pageCount = 0;

        await using var stream = file.OpenReadStream();
        var document = PdfReader.Open(stream, PdfDocumentOpenMode.ReadOnly);
        pageCount = document.PageCount;

        return pageCount;
    }
    
    private async Task HandleNewFileEventAsync(PdfFile file)
    {
        await _bus.PubSub.PublishAsync<FileCreatedMessage>(new FileCreatedMessage
        {
            Id = file.Id,
            AttId = file.AttId,
            AuthorId = file.AuthorId,
            FileName = file.FileName,
            CreatedAt = file.CreatedAt,
            Pages = file.Pages,
        });
    }   
    
    private async Task HandleDeletedFileEvent(int fileId)
    {
        await _bus.PubSub.PublishAsync<FileDeletedMessage>(new FileDeletedMessage
        {
            Id = fileId,
        });
    }
}
