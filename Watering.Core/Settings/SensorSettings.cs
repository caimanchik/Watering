namespace Watering.Core.Settings;

public record SensorSettings : SettingsBase
{
    public required int MeasurementPeriodSeconds { get; set; } = 5;
}