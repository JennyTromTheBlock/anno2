using Application.Domains.DTOs;

namespace Application.Interfaces.Services;

public interface IElasticSearchService
{
    Task IndexPdfAsync(byte[] pdfBytes, string documentId, string? caseId = null, string? attachmentId = null, string? fileName = null);

    Task<IEnumerable<PdfWord>> SearchWordPositionsAsync(CaseSearchQueryDto dto);
    Task<IEnumerable<PdfWithWordsReturnDto>> GetPdfWithWordsAsync(CaseSearchQueryDto dto);
    
    Task InitElasticSearch();

}