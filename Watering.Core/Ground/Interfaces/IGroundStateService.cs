using Microsoft.Extensions.Hosting;

namespace Watering.Core.Ground.Interfaces;

internal interface IGroundStateService : IHostedService
{
    float Humidity { get; }
    float IncreaseHumidity(float delta);
}