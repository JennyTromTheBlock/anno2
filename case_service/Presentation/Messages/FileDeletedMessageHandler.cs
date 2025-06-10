using Application.Domain.Messages;
using Application.Services.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Presentation.Messages.options;

namespace Presentation.Messages;

public class FileDeletedMessageHandler : BackgroundService, IMessageHandler<FileDeletedMessage>
{
    private readonly IOptions<RabbitMqOptions> _options;
    private IBus _bus;
    private readonly IServiceScopeFactory _scopeFactory;


    public FileDeletedMessageHandler(IOptions<RabbitMqOptions> options, IServiceScopeFactory scopeFactory)
    {
        _options = options;
        _scopeFactory = scopeFactory;

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

    public async Task HandleAsync(FileDeletedMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPdfFileInfoService>();

        await service.DeleteAsync(message.Id);
        await Task.CompletedTask;
    }
}
