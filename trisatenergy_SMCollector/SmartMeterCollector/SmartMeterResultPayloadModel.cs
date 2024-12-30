using System;
using System.Collections.Generic;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.IO;
using trisatenergy_smartmeters.SmartMeterSimulation;

namespace trisatenergy_SMCollector.SmartMeterCollector
{
    public class SmartMeterResultPayloadModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Timestamp { get; set; }

        public double TotalProduction { get; set; }

        public double SolarProduction { get; set; }
        public double WindProduction { get; set; }
        public double OtherProduction { get; set; }

        public double TotalConsumption { get; set; }
        public bool MaintenanceMode { get; set; }

        public static async Task<SmartMeterResultPayloadModel> FromPayload(SmartMeterResultPayload payload, IMongoCollection<SmartMeterResultPayloadModel> collection)
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

            try
            {
                await collection.InsertOneAsync(model);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                Console.WriteLine($"Duplicate key error: {ex.WriteError.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write error: {ex.Message}");
            }

            return model;
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
}