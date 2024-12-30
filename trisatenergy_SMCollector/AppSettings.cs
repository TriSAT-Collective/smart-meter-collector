namespace trisatenergy_SMCollector;

public class AppSettings
{
    public RabbitMQSettings RabbitMQ { get; set; }
    public MiscSettings Misc { get; set; }

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

}