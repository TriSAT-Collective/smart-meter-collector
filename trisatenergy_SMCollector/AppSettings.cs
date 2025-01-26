namespace trisatenergy_SMCollector;

public class AppSettings
{
    public RabbitMQSettings RabbitMQ { get; init; }
    public MiscSettings Misc { get; init; }
    public LoggingSettings Logging { get; init; }
    public MongoDBSettings MongoDB { get; init; }

    public class RabbitMQSettings
    {
        public Uri Uri { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }

    public class MiscSettings
    {
        public bool MaintenanceMode { get; set; }
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }

        public class LogLevelSettings
        {
            public string Default { get; set; }
        }
    }

    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}