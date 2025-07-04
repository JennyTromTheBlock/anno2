using Application.Domains.DTOs;

namespace Application.Interfaces.Services;

public interface IElasticSearchService
{
    Task IndexPdfAsync(byte[] pdfBytes, string documentId, string? caseId = null, string? attachmentId = null, string? fileName = null);

    Task<IEnumerable<PdfWithWordsReturnDto>> GetPdfWithWordsAsync(CaseSearchQueryDto dto);
    
    Task InitElasticSearch();
    
    //endpoints for removing files by ids
    Task<bool> DeleteByCaseIdAsync(string caseId);
    Task<bool> DeleteByAttachmentIdAsync(string attachmentId);
    Task<bool> DeleteByDocumentIdAsync(string documentId);



}