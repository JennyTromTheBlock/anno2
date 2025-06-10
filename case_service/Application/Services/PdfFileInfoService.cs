using Application.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

// PdfFileInfoService.cs
using Application.Domain.Entities;

public class PdfFileInfoService : IPdfFileInfoService
{
    private readonly IPdfFileInfoRepository _repository;

    public PdfFileInfoService(IPdfFileInfoRepository repository)
    {
        _repository = repository;
    }

    public async Task<PdfFileInfo?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(PdfFileInfo pdf)
    {
        await _repository.CreateAsync(pdf);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
