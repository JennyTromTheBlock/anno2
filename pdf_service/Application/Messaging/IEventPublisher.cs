namespace Application.Messaging;

public interface IEventPublisher : IAsyncDisposable
{
    Task PublishAsync<T>(string routingKey, T message) where T : class;
    Task InitializeAsync();
}
