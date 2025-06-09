
public class CaseEventListener : IEventConsumer
{
    private readonly RabbitMqConsumer<DeleteEvent> _consumer;

    public CaseEventListener(
        ConnectionFactory factory,
        ICaseService caseService) // din egen service til at håndtere sletninger etc
    {
        _consumer = new RabbitMqConsumer<DeleteEvent>(
            factory,
            "delete-event-queue",
            async (deleteEvent) =>
            {
                // Her kalder du logik til sletning i din case service
                await caseService.HandleDeleteAsync(deleteEvent.Id);
            });
    }

    public Task StartAsync(CancellationToken cancellationToken = default) => _consumer.StartAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken = default) => _consumer.StopAsync(cancellationToken);

    public ValueTask DisposeAsync() => _consumer.DisposeAsync();
}
