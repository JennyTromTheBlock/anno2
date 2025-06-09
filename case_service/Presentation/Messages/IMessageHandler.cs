namespace Presentation.Messages;

public interface IMessageHandler<T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}
