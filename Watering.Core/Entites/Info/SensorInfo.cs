namespace Watering.Core.Entites.Info;

public class SensorInfo : InfoBase
{
    public required float Humidity { get; set; }
    public required int MeasurementPeriodSeconds { get; set; }
}