namespace trisatenergy_smartmeters.SmartMeterSimulation;
/// <summary>
/// Represents the types of energy sources.
/// </summary>
public enum EnergySourceType
{   /// <summary>
    /// Solar energy source.
    /// </summary>
    Solar,
    /// <summary>
    /// Wind energy source.
    /// </summary>
    Wind,
    /// <summary>
    /// Other types of energy sources.
    /// </summary>
    Other
}
/// <summary>
/// Represents the payload for smart meter results.
/// </summary>
public class SmartMeterResultPayload
{   /// <summary>
    /// Gets the timestamp of the payload.
    /// </summary>
    public DateTime Timestamp { get; init; }
    /// <summary>
    /// Gets the total production value.
    /// </summary>
    public double TotalProduction { get; init; }
    /// <summary>
    /// Gets the production values by energy source.
    /// </summary>
    public Dictionary<EnergySourceType, double> ProductionBySource { get; init; }
    /// <summary>
    /// Gets the total consumption value.
    /// </summary>
    public double TotalConsumption { get; init; }
    /// <summary>
    /// Gets a value indicating whether the maintenance mode is enabled.
    /// </summary>
    public bool MaintenanceMode { get; init; }
}