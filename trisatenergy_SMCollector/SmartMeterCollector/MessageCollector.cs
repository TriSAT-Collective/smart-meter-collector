using System;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IO;

namespace SmartMeterCollector
{
    public class MessageCollector : IAsyncDisposable
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _virtualHost;
        private readonly string _queueName;
        private IConnection _connection;
        private IChannel _channel; // Changed from IModel to IChannel

        public MessageCollector()
        {
            // Load environment variables
            LoadEnvFile();

            // Initialize RabbitMQ settings from environment variables
            _hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";  // Default to localhost if not set
            _userName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest";      // Default to guest if not set
            _password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";     // Default to guest if not set
            _virtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VHOST") ?? "/";     // Default to "/" if not set
            _queueName = Environment.GetEnvironmentVariable("RABBITMQ_QUEUE_OK") ?? "ok_queue"; // Default to "ok_queue" if not set
        }

        public async Task StartCollectingAsync()
        {
            Console.WriteLine("_userName: " + _userName);
            
            
            // Setup RabbitMQ connection and channel
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://localhost"), // Simplified to use only "localhost"
                VirtualHost = _virtualHost,         // Optional, use the appropriate virtual host
                UserName = _userName,               // RabbitMQ username
                Password = _password                // RabbitMQ password
            };

            // Asynchronously create the connection and channel
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();  // Changed from CreateModelAsync() to CreateChannelAsync()

            // Declare the queue (ensure it exists before consuming)
            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true, // Make sure the queue survives server restarts
                exclusive: false, // Queue will be shared between consumers
                autoDelete: false, // Do not delete the queue when the last consumer is gone
                arguments: null,
                passive: true); // Set passive to true to check if the queue exists without modifying it
                                // No extra arguments

            // Define the consumer event
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received message: " + message);
            };

            // Start consuming messages from the specified "OK" queue
            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
        }

        private void LoadEnvFile()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string envFilePath = TraverseForEnvFile(baseDirectory); // Use the method to find the .env file

            if (envFilePath != null)
            {
                Console.WriteLine("Found .env file at: " + envFilePath);
                Env.Load(envFilePath); // Load the .env file
            }
            else
            {
                Console.WriteLine("No .env file found.");
            }
        }

        private string TraverseForEnvFile(string startingDirectory)
        {
            string directory = startingDirectory;

            while (directory != null)
            {
                // Look for the .env file in the current directory
                string envFilePath = Path.Combine(directory, ".env");
                if (File.Exists(envFilePath))
                {
                    return envFilePath; // Return the path if found
                }

                // Move up one level in the directory hierarchy
                directory = Directory.GetParent(directory)?.FullName;
            }

            // Return null if no .env file was found after traversing all directories
            return null;
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }
    }
}
