using Microsoft.Extensions.Hosting;
using Watering.Core.Entites.Enums;
using Watering.Core.ServicesBase.Interfaces;

namespace Watering.Core.Controllers.Interfaces;

internal interface ISprinklerService : ISettingsChangeService, IHostedService
{
    SprinklerState State { get; }

    Task TurnOn();

    Task TurnOff();
}