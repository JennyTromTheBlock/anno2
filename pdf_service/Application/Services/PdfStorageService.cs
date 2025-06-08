namespace Application.Services;

using Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;


public class PdfStorageService : IPdfStorageService
{
    private readonly IPdfStorageRepository _repository;

    public PdfStorageService(IPdfStorageRepository repository)
    {
        _repository = repository;
    }

    public Task<string> SavePdfAsync(IFormFile file)
    {
        return _repository.SavePdfAsync(file);
    }

    public Task<byte[]> GetPdfAsync(string relativePath)
    {
        return _repository.GetPdfAsync(relativePath);
    }

    public Task<List<byte[]>> GetMultiplePdfsAsync(List<string> relativePaths)
    {
        return _repository.GetMultiplePdfsAsync(relativePaths);
    }
}
