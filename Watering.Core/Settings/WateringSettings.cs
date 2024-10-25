using Watering.Core.Settings.Enums;

namespace Watering.Core.Settings;

public record WateringSettings : SettingsBase
{
    public required SprinklerMode SprinklerMode { get; set; } = SprinklerMode.Auto;
    public required float MinHumidityLevel { get; set; } = 1;
    public required float MaxHumidityLevel { get; set; } = 10;
}