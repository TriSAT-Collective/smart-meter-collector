using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_SMCollector.SmartMeterCollector;

public class MessageCollector
{
    private readonly IMongoCollection<SmartMeterResultPayloadModel> _collection;
    private readonly ILogger<MessageCollector> _logger;
    private readonly AppSettings _settings;

    private IChannel _channel; // Changed from IModel to IChannel
    private IConnection _connection;

    public MessageCollector(IOptions<AppSettings> settings, ILogger<MessageCollector> logger,
        IMongoCollection<SmartMeterResultPayloadModel> collection)
    {
        _settings = settings.Value;
        _logger = logger;
        _collection = collection;
    }

    public async Task Stop()
    {
        _logger.LogInformation("Message collector stopped.");
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

    public async Task Start()
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
        consumer.ReceivedAsync += async (x, ea) =>
        {
            var body = ea.Body.ToArray();
            var str = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<SmartMeterResultPayload>(str);
            _logger.LogInformation($"Received message: {message}");
            SmartMeterResultPayloadModel model = SmartMeterResultPayloadModel.FromPayload(message);
            await model.Insert(_collection);
        };

        // Start consuming messages from the specified "OK" queue
        await _channel.BasicConsumeAsync(_settings.RabbitMQ.QueueName, true, consumer);
    }
}