namespace trisatenergy_smartmeters.SmartMeterSimulation;

public enum EnergySourceType
{
    Solar,
    Wind,
    Other
}

public class SmartMeterResultPayload
{
    public DateTime Timestamp { get; init; }
    public double TotalProduction { get; init; }
    public Dictionary<EnergySourceType, double> ProductionBySource { get; init; }
    public double TotalConsumption { get; init; }
    public bool MaintenanceMode { get; init; }
}