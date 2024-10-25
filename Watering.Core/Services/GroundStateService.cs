using Watering.Core.Entites;
using Watering.Core.Services.Interfaces;

namespace Watering.Core.Services;

public class GroundStateService : IGroundStateService
{
    private readonly GroundState _state = new();

    public GroundStateService() => InitGroundTimer();

    public float Humidity => _state.Humidity;
    
    public float IncreaseHumidity(float delta = 1)
    {
        _state.Humidity += delta;
        return _state.Humidity;
    }

    private async void InitGroundTimer()
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync()) 
            IncreaseHumidity(-0.01f);
    }
}