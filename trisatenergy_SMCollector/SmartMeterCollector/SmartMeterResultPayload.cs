namespace trisatenergy_smartmeters.SmartMeterSimulation;

public enum EnergySourceType
{
    Solar,
    Wind,
    Other
}

public class SmartMeterResultPayload
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double TotalProduction { get; set; }
    public Dictionary<EnergySourceType, double> ProductionBySource { get; set; }
    public double TotalConsumption { get; set; }
    public bool MaintenanceMode { get; set; }
}