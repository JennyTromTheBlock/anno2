namespace Application.Messaging.Events;

public class PdfFileDeletedEvent : IEvent
{
    public string EventType => nameof(FileCreatedEvent);

    public int FileId { get; set; }


    public PdfFileDeletedEvent(int fileId)
    {
        this.FileId = fileId;
    }
}