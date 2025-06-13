namespace DefaultNamespace;

using Application.Parsers;

public interface IElasticSearchService
{
    Task IndexPdfAsync(byte[] pdfBytes, string documentId);
    Task<IEnumerable<PdfWordEntry>> SearchAsync(string word, string? documentId = null);
}
