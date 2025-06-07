namespace Application.Domain.DTOs;

public class AttachmentCreateRequest
{
    public int CaseId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? ImgPath { get; set; }
}

public class AttachmentUpdateRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImgPath { get; set; }
}
