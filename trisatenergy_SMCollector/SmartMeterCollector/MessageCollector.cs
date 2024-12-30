using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace trisatenergy_SMCollector.SmartMeterCollector;

public class MessageCollector
{
    private readonly ILogger<MessageCollector> _logger;
    private readonly AppSettings _settings;

    private IChannel _channel; // Changed from IModel to IChannel
    private IConnection _connection;

    public MessageCollector(IOptions<AppSettings> settings, ILogger<MessageCollector> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task StartCollectingAsync()
    {
        _logger.LogInformation("Starting to collect messages...");
        // Setup RabbitMQ connection and channel
        var factory = new ConnectionFactory
        {
            Uri = _settings.RabbitMQ.Uri,
            VirtualHost = _settings.RabbitMQ.VirtualHost,
            UserName = _settings.RabbitMQ.Username,
            Password = _settings.RabbitMQ.Password
        };

        // Asynchronously create the connection and channel
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Define the consumer event
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");
            return Task.CompletedTask;
        };

        // Start consuming messages from the specified "OK" queue
        await _channel.BasicConsumeAsync(_settings.RabbitMQ.QueueName, true, consumer);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _channel.DisposeAsync();
        await _connection.CloseAsync();
        await _connection.DisposeAsync();
    }
}