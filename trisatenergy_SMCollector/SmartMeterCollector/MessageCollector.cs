using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace trisatenergy_SMCollector.SmartMeterCollector
{
    public class MessageCollector : IAsyncDisposable
    {
        
        private readonly ILogger<MessageCollector> _logger;
        private readonly AppSettings _settings;
        
        private IConnection _connection;
        private IChannel _channel; // Changed from IModel to IChannel

        public MessageCollector(IOptions<AppSettings> settings, ILogger<MessageCollector> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            // Setup RabbitMQ connection and channel
            var factory = new ConnectionFactory()
            {
                Uri = _settings.RabbitMQ.Uri,
                VirtualHost =   _settings.RabbitMQ.VirtualHost,
                UserName = _settings.RabbitMQ.Username,
                Password = _settings.RabbitMQ.Password
            };

            // Asynchronously create the connection and channel
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();  
        }

        public async Task StartCollectingAsync()
        {

            await InitializeAsync();
            
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
            await _channel.BasicConsumeAsync(queue: _settings.RabbitMQ.QueueName, autoAck: true, consumer: consumer);
        }

        public async ValueTask DisposeAsync()
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}
