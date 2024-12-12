using Watering.Core.Entites;
using Watering.Core.Ground.Interfaces;

namespace Watering.Core.Ground;

public class GroundStateService : IGroundStateService
{
    private readonly GroundState _state = new();
    private PeriodicTimer? _timer;

    public float Humidity => _state.Humidity;
    
    public float IncreaseHumidity(float delta = 1)
    {
        _state.Humidity += delta;
        return _state.Humidity;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        InitGroundTimer();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) 
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }
    
    private async void InitGroundTimer()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await _timer.WaitForNextTickAsync()) 
            IncreaseHumidity(-0.01f);
    }
}