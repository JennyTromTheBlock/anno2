
using Application.Domain.Messages;
using Application.Domain.Entities;
using Application.Services.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Presentation.Messages.options;

namespace Presentation.Messages;

public class FileCreatedMessageHandler : BackgroundService, IMessageHandler<FileCreatedMessage>
{
    private readonly IOptions<RabbitMqOptions> _options;
    private readonly IServiceScopeFactory _scopeFactory;
    private IBus _bus;

    public FileCreatedMessageHandler(IOptions<RabbitMqOptions> options, IServiceScopeFactory scopeFactory)
    {
        _options = options;
        _scopeFactory = scopeFactory;
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

    public async Task HandleAsync(FileCreatedMessage message, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IPdfFileInfoService>();

        var file = new PdfFileInfo
        {
            Id = message.Id,
            AttId = message.AttId,
            AuthorId = message.AuthorId,
            FileName = message.FileName,
            CreatedAt = message.CreatedAt,
            Pages = message.Pages,
        };

        await service.CreateAsync(file);
    }
}
