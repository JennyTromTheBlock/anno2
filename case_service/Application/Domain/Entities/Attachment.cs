namespace Application.Domain.Entities;

public class AttachmentBase
{
    public int CaseId { get; set; }
    public string Title { get; set; }
    public int? Author { get; set; }
    public int? LastEditBy { get; set; }
    public int Position { get; set; } = 0;
    public string? Description { get; set; }
    public string? ImgPath { get; set; }
}

public class Attachment : AttachmentBase
{
    public int? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}