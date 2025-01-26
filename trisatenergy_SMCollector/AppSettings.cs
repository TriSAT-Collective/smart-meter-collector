namespace trisatenergy_SMCollector;
/// <summary>
/// Represents the application settings.
/// </summary>
public class AppSettings
{   /// <summary>
    /// Gets or sets the RabbitMQ settings.
    /// </summary>
    public RabbitMQSettings RabbitMQ { get; init; }
    /// <summary>
    /// Gets or sets the miscellaneous settings.
    /// </summary>
    public MiscSettings Misc { get; init; }
    /// <summary>
    /// Gets or sets the logging settings.
    /// </summary>
    public LoggingSettings Logging { get; init; }
    /// <summary>
    /// Gets or sets the MongoDB settings.
    /// </summary>
    public MongoDBSettings MongoDB { get; init; }
    /// <summary>
    /// Represents the RabbitMQ settings.
    /// </summary>
    public class RabbitMQSettings
    {   /// <summary>
        /// Gets or sets the URI of the RabbitMQ server.
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// Gets or sets the virtual host.
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// Gets or sets the username for RabbitMQ.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password for RabbitMQ.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the name of the RabbitMQ queue.
        /// </summary>
        public string QueueName { get; set; }
    }
    /// <summary>
    /// Represents miscellaneous settings.
    /// </summary>
    public class MiscSettings
    {   /// <summary>
        /// Gets or sets a value indicating whether the maintenance mode is enabled.
        /// </summary>
        public bool MaintenanceMode { get; set; }
    }
    /// <summary>
    /// Represents the logging settings.
    /// </summary>
    public class LoggingSettings
    {   /// <summary>
        /// Gets or sets the log level settings.
        /// </summary>
        public LogLevelSettings LogLevel { get; set; }
        /// <summary>
        /// Represents the log level settings.
        /// </summary>
        public class LogLevelSettings
        {   /// <summary>
            /// Gets or sets the default log level.
            /// </summary>
            public string Default { get; set; }
        }
    }
    /// <summary>
    /// Represents the MongoDB settings.
    /// </summary>
    public class MongoDBSettings
    {   /// <summary>
        /// Gets or sets the connection string for MongoDB.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Gets or sets the name of the MongoDB collection.
        /// </summary>
        public string CollectionName { get; set; }
    }
}