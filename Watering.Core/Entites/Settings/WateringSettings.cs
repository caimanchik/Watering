using Watering.Core.Entites.Enums;

namespace Watering.Core.Entites.Settings;

public record WateringSettings : SettingsBase
{
    public required SprinklerMode SprinklerMode { get; set; } = SprinklerMode.Auto;
    public required float MinHumidityLevel { get; set; } = 1;
    public required float MaxHumidityLevel { get; set; } = 10;
}