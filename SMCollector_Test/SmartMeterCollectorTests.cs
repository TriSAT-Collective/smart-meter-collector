using System.Text.Json;
using MongoDB.Driver;
using Moq;
using trisatenergy_smartmeters.SmartMeterSimulation;
using trisatenergy_SMCollector.SmartMeterCollector;

namespace SMCollector_Test;

public class SmartMeterResultPayloadModelTests
{
    [Fact]
    public Task FromPayload_ShouldMapPayloadToModel()
    {
        // Arrange
        var payload = new SmartMeterResultPayload
        {
            Timestamp = DateTime.UtcNow,
            TotalProduction = 100,
            ProductionBySource = new Dictionary<EnergySourceType, double>
            {
                { EnergySourceType.Solar, 50 },
                { EnergySourceType.Wind, 30 }
            },
            TotalConsumption = 80,
            MaintenanceMode = false
        };

        // Act
        SmartMeterResultPayloadModel model = SmartMeterResultPayloadModel.FromPayload(payload);

        // Assert
        Assert.Equal(payload.Timestamp, model.Timestamp);
        Assert.Equal(payload.TotalProduction, model.TotalProduction);
        Assert.Equal(50, model.SolarProduction);
        Assert.Equal(30, model.WindProduction);
        Assert.Equal(0, model.OtherProduction);
        Assert.Equal(payload.TotalConsumption, model.TotalConsumption);
        Assert.Equal(payload.MaintenanceMode, model.MaintenanceMode);
        return Task.CompletedTask;
    }

    [Fact]
    public void ToJson_ShouldSerializeModelToJson()
    {
        // Arrange
        var model = new SmartMeterResultPayloadModel
        {
            Timestamp = DateTime.UtcNow,
            TotalProduction = 100,
            SolarProduction = 50,
            WindProduction = 30,
            OtherProduction = 20,
            TotalConsumption = 80,
            MaintenanceMode = false
        };

        // Act
        var json = model.ToJson();

        // Assert
        var expectedJson = JsonSerializer.Serialize(model);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public async Task ExportToJsonFile_ShouldWriteJsonToFile()
    {
        // Arrange
        var model = new SmartMeterResultPayloadModel
        {
            Timestamp = DateTime.UtcNow,
            TotalProduction = 100,
            SolarProduction = 50,
            WindProduction = 30,
            OtherProduction = 20,
            TotalConsumption = 80,
            MaintenanceMode = false
        };

        var filePath = "test.json";

        // Act
        await model.ExportToJsonFile(filePath);

        // Assert
        var json = await File.ReadAllTextAsync(filePath);
        var expectedJson = model.ToJson();
        Assert.Equal(expectedJson, json);

        // Cleanup
        File.Delete(filePath);
    }
}