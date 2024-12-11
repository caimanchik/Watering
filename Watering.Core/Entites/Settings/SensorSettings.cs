namespace Watering.Core.Entites.Settings;

public record SensorSettings : SettingsBase
{
    public required int MeasurementPeriodSeconds { get; set; }
}