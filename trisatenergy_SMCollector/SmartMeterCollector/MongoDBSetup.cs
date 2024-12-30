
using MongoDB.Driver;
using System.Threading.Tasks;
using trisatenergy_SMCollector.SmartMeterCollector;
namespace trisatenergy_SMCollector
{
    public static class MongoDBSetup
    {
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