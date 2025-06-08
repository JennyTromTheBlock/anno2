namespace Application.Interfaces.Services;

using Microsoft.AspNetCore.Http;


public interface IPdfStorageService
{
    Task<string> SavePdfAsync(IFormFile file);
    Task<byte[]> GetPdfAsync(string relativePath);
    Task<List<byte[]>> GetMultiplePdfsAsync(List<string> relativePaths);
}
