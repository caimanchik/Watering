using Watering.Core.Settings.Enums;

namespace Watering.Core.Services.Interfaces;

internal interface ISprinklerService : ISettingsChangeService
{
    SprinklerState State { get; }

    void TurnOn();

    void TurnOff();
}