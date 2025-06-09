
using Application.Domain.Messages;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Presentation.Messages.options;

namespace Presentation.Messages;

public class FileCreatedMessageHandler : BackgroundService, IMessageHandler<FileCreatedMessage>
{
    private readonly IOptions<RabbitMqOptions> _options;
    private IBus _bus;

    public FileCreatedMessageHandler(IOptions<RabbitMqOptions> options) 
    {
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus = RabbitHutch.CreateBus(_options.Value.ConnectionString);

        _bus.PubSub.Subscribe<FileCreatedMessage>(
            "",
            msg => HandleAsync(msg, stoppingToken),
            cancellationToken: stoppingToken
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public Task HandleAsync(FileCreatedMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[FileCreated] Received: {message.FileName}");
        return Task.CompletedTask;
    }
}
