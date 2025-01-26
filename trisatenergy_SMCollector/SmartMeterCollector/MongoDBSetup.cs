
using MongoDB.Driver;
using System.Threading.Tasks;
using trisatenergy_SMCollector.SmartMeterCollector;
namespace trisatenergy_SMCollector
{   /// <summary>
    /// Provides methods for setting up MongoDB.
    /// </summary>
    public static class MongoDBSetup
    {
        /// <summary>
        /// Initializes the MongoDB collection for SmartMeterResultPayloadModel.
        /// </summary>
        /// <param name="appSettings">The application settings containing MongoDB configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the MongoDB collection.</returns>
        public static async Task<IMongoCollection<SmartMeterResultPayloadModel>> InitializeMongoDB(AppSettings appSettings)
        {
            var client = new MongoClient(appSettings.MongoDB.ConnectionString);
            var database = client.GetDatabase(appSettings.MongoDB.DatabaseName);
            var collection = database.GetCollection<SmartMeterResultPayloadModel>(appSettings.MongoDB.CollectionName);

            // Create a compound unique index on the Timestamp and Geometry fields
            var indexKeysDefinition = Builders<SmartMeterResultPayloadModel>.IndexKeys
                .Ascending(model => model.Timestamp);
            var indexOptions = new CreateIndexOptions { Unique = true };
            await collection.Indexes.CreateOneAsync(new CreateIndexModel<SmartMeterResultPayloadModel>(indexKeysDefinition, indexOptions));

            return collection;
        }
    }
}