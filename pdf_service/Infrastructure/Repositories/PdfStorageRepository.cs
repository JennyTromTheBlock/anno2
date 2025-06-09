using Application.Interfaces.Repositories;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Repositories;

using Microsoft.AspNetCore.Http;


public class PdfStorageRepository : IPdfStorageRepository
{
    private readonly string _pdfDirectory;

    public PdfStorageRepository(IHostEnvironment env)
    {
        _pdfDirectory = "/dbdata/pdfs";
        if (!Directory.Exists(_pdfDirectory))
            Directory.CreateDirectory(_pdfDirectory);
    }

    public async Task<string> SavePdfAsync(IFormFile file)
    {
        Console.WriteLine(file.FileName);
        var fileName = $"{Guid.NewGuid()}.pdf";
        var filePath = Path.Combine(_pdfDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    public async Task<byte[]> GetPdfAsync(string relativePath)
    {
        var fullPath = Path.Combine(_pdfDirectory, relativePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("PDF ikke fundet", fullPath);

        return await File.ReadAllBytesAsync(fullPath);
    }

    public async Task<List<byte[]>> GetMultiplePdfsAsync(List<string> relativePaths)
    {
        var result = new List<byte[]>();

        foreach (var path in relativePaths)
        {
            try
            {
                var fullPath = Path.Combine(_pdfDirectory, path);
                if (File.Exists(fullPath))
                {
                    var content = await File.ReadAllBytesAsync(fullPath);
                    result.Add(content);
                }
            }
            catch
            {
                // valgfrit: log eller skip
            }
        }

        return result;
    }
}
