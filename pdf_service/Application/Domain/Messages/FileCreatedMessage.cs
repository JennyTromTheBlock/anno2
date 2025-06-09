
namespace Application.Domain.Messages;

public class FileCreatedMessage
{
    public int Id { get; set; }
    public int AttId { get; set; }

    
    public int AuthorId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int Pages { get; set; }
}