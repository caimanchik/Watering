using Watering.Core.Entites.Enums;

namespace Watering.Core.Entites.Info;

public class SprinklerInfo : InfoBase
{
    public required SprinklerState State { get; set; } = SprinklerState.Off;
    public required float Intensivity { get; set; } = 0.1f;
}