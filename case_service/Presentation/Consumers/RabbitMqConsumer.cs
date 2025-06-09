namespace Presentation.Consumers;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqConsumer<T> : IEventConsumer where T : class
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly string _queueName;
    private readonly Func<T, Task> _handleMessageAsync;

    public RabbitMqConsumer(
        ConnectionFactory factory, 
        string queueName,
        Func<T, Task> handleMessageAsync)
    {
        _factory = factory;
        _queueName = queueName;
        _handleMessageAsync = handleMessageAsync;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(body);
            if (message != null)
            {
                await _handleMessageAsync(message);
            }
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
            autoAck: false,
            consumer: consumer);

        await Task.CompletedTask; // just to satisfy async signature
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
            _channel.Dispose();
        }

        if (_connection != null && _connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }
}
