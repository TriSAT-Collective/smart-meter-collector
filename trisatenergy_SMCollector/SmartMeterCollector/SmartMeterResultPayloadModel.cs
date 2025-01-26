using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_SMCollector.SmartMeterCollector;

public class SmartMeterResultPayloadModel
{
    [BsonId] public ObjectId Id { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; init; }

    public double TotalProduction { get; init; }

    public double SolarProduction { get; init; }
    public double WindProduction { get; init; }
    public double OtherProduction { get; init; }

    public double TotalConsumption { get; init; }
    public bool MaintenanceMode { get; init; }

    public static SmartMeterResultPayloadModel FromPayload(SmartMeterResultPayload? payload)
    {
        var model = new SmartMeterResultPayloadModel
        {
            Timestamp = payload.Timestamp,
            TotalProduction = payload.TotalProduction,
            SolarProduction = payload.ProductionBySource.TryGetValue(EnergySourceType.Solar, out var solar) ? solar : 0,
            WindProduction = payload.ProductionBySource.TryGetValue(EnergySourceType.Wind, out var wind) ? wind : 0,
            OtherProduction = payload.ProductionBySource.TryGetValue(EnergySourceType.Other, out var other) ? other : 0,
            TotalConsumption = payload.TotalConsumption,
            MaintenanceMode = payload.MaintenanceMode
        };

        return model;
    }


    public Task Insert(IMongoCollection<SmartMeterResultPayloadModel> collection)
    {
        collection.InsertOneAsync(this);
        return Task.CompletedTask;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public async Task ExportToJsonFile(string filePath)
    {
        var jsonString = ToJson();
        await File.WriteAllTextAsync(filePath, jsonString);
    }
}