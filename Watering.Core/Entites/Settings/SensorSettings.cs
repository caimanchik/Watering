namespace Watering.Core.Entites.Settings;

public record SensorSettings : SettingsBase
{
    public required int MeasurementPeriodInSeconds { get; set; }
}