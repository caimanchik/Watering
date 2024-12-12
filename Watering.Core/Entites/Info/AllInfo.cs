using System.Text;
using Watering.Core.Entites.Enums;

namespace Watering.Core.Entites.Info;

public partial class AllInfo(SensorInfo? sensorInfo, SprinklerInfo? sprinklerInfo, WateringInfo? wateringInfo)
{
    public SensorInfo? SensorInfo { get; } = sensorInfo;
    public SprinklerInfo? SprinklerInfo { get; } = sprinklerInfo;
    public WateringInfo? WateringInfo { get; } = wateringInfo;
}

public partial class AllInfo
{
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (SensorInfo is not null)
        {
            sb.Append($"Влажность: {SensorInfo.Humidity}\n");
            sb.Append($"Период измерения влажности: {SensorInfo.MeasurementPeriodSeconds}\n");
        }

        if (SprinklerInfo is not null)
        {
            sb.Append($"Состяние поливателя: {SprinklerInfo.State}\n");
            if (SprinklerInfo.State.HasFlag(SprinklerState.On))
                sb.Append($"Интенсивность полива: {SprinklerInfo.Intensivity}\n");
        }

        if (WateringInfo is not null)
        {
            sb.Append($"Режим полива: {WateringInfo.SprinklerMode}\n");
            if (WateringInfo.SprinklerMode.HasFlag(SprinklerMode.Auto))
            {
                sb.Append($"Минимальная влажность: {WateringInfo.MinHumidityLevel}\n");
                sb.Append($"Максимальная влажность: {WateringInfo.MaxHumidityLevel}\n");
            }
        }
        
        return sb.ToString();
    }
}