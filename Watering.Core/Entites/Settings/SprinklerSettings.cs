using Watering.Core.Entites.Enums;

namespace Watering.Core.Entites.Settings;

public record SprinklerSettings : SettingsBase
{
    public required float Intensity { get; set; } = 0.1f;
    public SprinklerState? State { get; set; }
}