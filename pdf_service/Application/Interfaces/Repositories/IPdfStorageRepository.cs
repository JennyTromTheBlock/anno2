namespace Application.Interfaces.Repositories;

using Microsoft.AspNetCore.Http;

public interface IPdfStorageRepository
{
    Task<string> SavePdfAsync(IFormFile file);
    Task<byte[]> GetPdfAsync(string relativePath);
    Task<List<byte[]>> GetMultiplePdfsAsync(List<string> relativePaths);
}
