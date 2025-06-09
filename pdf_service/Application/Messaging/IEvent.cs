namespace Application.Messaging;

public interface IEvent
{
    string EventType { get; }
}