using MongoDB.Driver;
using trisatenergy_SMCollector.SmartMeterCollector;

namespace trisatenergy_SMCollector;

public static class MongoDBSetup
{
    public static async Task<IMongoCollection<SmartMeterResultPayloadModel>> InitializeMongoDB(AppSettings appSettings)
    {
        var client = new MongoClient(appSettings.MongoDB.ConnectionString);
        IMongoDatabase? database = client.GetDatabase(appSettings.MongoDB.DatabaseName);
        var collection = database.GetCollection<SmartMeterResultPayloadModel>(appSettings.MongoDB.CollectionName);


        var indexKeysDefinition = Builders<SmartMeterResultPayloadModel>.IndexKeys
            .Ascending(model => model.Timestamp)
            .Ascending(model => model.SmartMeterId);

        var indexOptions = new CreateIndexOptions { Unique = true };
        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<SmartMeterResultPayloadModel>(indexKeysDefinition, indexOptions));

        return collection;
    }
}