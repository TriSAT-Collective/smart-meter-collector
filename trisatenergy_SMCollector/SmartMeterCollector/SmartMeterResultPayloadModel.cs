using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_SMCollector.SmartMeterCollector;
/// <summary>
/// Represents the model for smart meter result payload.
/// </summary>
public class SmartMeterResultPayloadModel
{   /// <summary>
    /// Gets or sets the unique identifier for the document.
    /// </summary>
    [BsonId] public ObjectId Id { get; set; }
    /// <summary>
    /// Gets the timestamp of the payload.
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; init; }
    /// <summary>
    /// Gets the total production value.
    /// </summary>
    public double TotalProduction { get; init; }
    /// <summary>
    /// Gets the solar production value.
    /// </summary>
    public double SolarProduction { get; init; }
    /// <summary>
    /// Gets the wind production value.
    /// </summary>
    public double WindProduction { get; init; }
    /// <summary>
    /// Gets the other production value.
    /// </summary>
    public double OtherProduction { get; init; }
    /// <summary>
    /// Gets the total consumption value.
    /// </summary>
    public double TotalConsumption { get; init; }
    /// <summary>
    /// Gets a value indicating whether the maintenance mode is enabled.
    /// </summary>
    public bool MaintenanceMode { get; init; }
    /// <summary>
    /// Creates a new instance of <see cref="SmartMeterResultPayloadModel"/> from the given payload.
    /// </summary>
    /// <param name="payload">The smart meter result payload.</param>
    /// <returns>A new instance of <see cref="SmartMeterResultPayloadModel"/>.</returns>
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

    /// <summary>
    /// Inserts the current model instance into the specified MongoDB collection.
    /// </summary>
    /// <param name="collection">The MongoDB collection.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task Insert(IMongoCollection<SmartMeterResultPayloadModel> collection)
    {
        collection.InsertOneAsync(this);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Exports the current model instance to a JSON file.
    /// </summary>
    /// <param name="filePath">The file path where the JSON should be saved.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
    /// <summary>
    /// Exports the current model instance to a JSON file.
    /// </summary>
    /// <param name="filePath">The file path where the JSON should be saved.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ExportToJsonFile(string filePath)
    {
        var jsonString = ToJson();
        await File.WriteAllTextAsync(filePath, jsonString);
    }
}