namespace Application.Messaging.Events;

public class FileCreatedEvent : IEvent
{
    public string EventType => nameof(FileCreatedEvent);

    public string FileName { get; set; }
    public int PageCount { get; set; }
    public int AuthorId { get; set; }

    public FileCreatedEvent(string fileName, int pageCount, int authorId)
    {
        FileName = fileName;
        PageCount = pageCount;
        AuthorId = authorId;
    }
}