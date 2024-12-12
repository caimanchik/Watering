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
            sb.AppendLine($"Влажность: {SensorInfo.Humidity}");
            sb.AppendLine($"Период измерения влажности: {SensorInfo.MeasurementPeriodSeconds}");
        }

        if (SprinklerInfo is not null)
        {
            sb.AppendLine($"Состяние поливателя: {SprinklerInfo.State}");
            if (SprinklerInfo.State.HasFlag(SprinklerState.On))
                sb.AppendLine($"Интенсивность полива: {SprinklerInfo.Intensivity}");
        }

        if (WateringInfo is not null)
        {
            sb.AppendLine($"Режим полива: {WateringInfo.Mode}");
            if (WateringInfo.Mode.HasFlag(SprinklerMode.Auto))
            {
                sb.AppendLine($"Минимальная влажность: {WateringInfo.MinHumidityLevel}");
                sb.AppendLine($"Максимальная влажность: {WateringInfo.MaxHumidityLevel}");
            }
        }
        
        return sb.ToString();
    }
}