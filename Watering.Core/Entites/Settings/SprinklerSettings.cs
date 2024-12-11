namespace Watering.Core.Entites.Settings;

public record SprinklerSettings : SettingsBase
{
    public required float Intensivity { get; set; } = 0.1f;
}