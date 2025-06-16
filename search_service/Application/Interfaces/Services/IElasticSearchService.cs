using Application.Domains.DTOs;

namespace Application.Interfaces.Services;

public interface IElasticSearchService
{
    Task IndexPdfAsync(byte[] pdfBytes, string documentId);
    Task<IEnumerable<PdfWordEntry>> SearchAsync(string word, string? documentId = null);
}