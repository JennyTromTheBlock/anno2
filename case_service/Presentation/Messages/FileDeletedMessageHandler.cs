using Application.Domain.Messages;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Presentation.Messages.options;

namespace Presentation.Messages;

public class FileDeletedMessageHandler : BackgroundService, IMessageHandler<FileDeletedMessage>
{
    private readonly IOptions<RabbitMqOptions> _options;
    private IBus _bus;

    public FileDeletedMessageHandler(IOptions<RabbitMqOptions> options)
    {
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus = RabbitHutch.CreateBus(_options.Value.ConnectionString);

        _bus.PubSub.Subscribe<FileDeletedMessage>(
            "",
            msg => HandleAsync(msg, stoppingToken),
            cancellationToken: stoppingToken
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public Task HandleAsync(FileDeletedMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[FileDeleted] Received: {message.Id}");
        return Task.CompletedTask;
    }
}
