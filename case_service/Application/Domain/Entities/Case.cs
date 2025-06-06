namespace Application.Domain.Entities
{
    public class Case
    {
        public int Id { get; set; }
        public string? CaseNumber { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public string? ImgPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        
        public DateTime? DeletedAt { get; set; }

    }
}