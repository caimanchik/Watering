namespace Watering.Core.Services.Interfaces;

internal interface IGroundStateService
{
    float Humidity { get; }
    float IncreaseHumidity(float delta);
}