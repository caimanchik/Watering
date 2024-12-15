using Microsoft.Extensions.Logging;
using Watering.Core.Client.Interfaces;
using Watering.Core.Controllers.Interfaces;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;
using Watering.Core.Ground.Interfaces;

namespace Watering.Core.Controllers;

internal class SprinklerService(
    IControlClient client,
    IGroundStateService groundStateService,
    ILogger<SprinklerService> logger)
    : ISprinklerService
{
    private const int WateringPeriodSeconds = 1;

    private readonly SprinklerSettings _settings = new()
    {
        Intensity = 0.1f
    };
    
    private PeriodicTimer? _timer;

    public void RegisterSettingsAction()
    {
        client.RegisterSettingsChange<SprinklerSettings>(TryUpdateSettings);
        return;

        async Task TryUpdateSettings(SprinklerSettings settings)
        {
            _settings.Intensity = settings.Intensity;
            if (settings.State is not null)
            {
                _settings.State = settings.State;
                if (settings.State.Value.HasFlag(SprinklerState.Off))
                    TurnOff();
                else
                    TurnOn();
            }
            // logger.LogInformation("Обновлены настройки поливателя");
            await SendInformation();
        }
    }

    public SprinklerState State { get; private set; } = SprinklerState.Off;

    public async Task TurnOn()
    {
        if (State is SprinklerState.On)
        {
            // logger.LogInformation("Поливатель включен");
            return;
        }
        State = SprinklerState.On;
        await SendInformation();
    }

    public async Task TurnOff()
    {
        if (State is SprinklerState.Off)
        {
            // logger.LogInformation("Поливатель выключен");
            return;
        }
        State = SprinklerState.Off;
        await SendInformation();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterSettingsAction();
        CreateWateringTimer();
        await SendInformation();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private async Task SendInformation()
    {
        await client.SendInfo(new SprinklerInfo
        {
            Intensivity = _settings.Intensity,
            State = State,
        });
    }

    private async void CreateWateringTimer()
    {
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(WateringPeriodSeconds));
        while (await _timer.WaitForNextTickAsync())
            OnTimerTick();
    }

    private void OnTimerTick()
    {
        if (State.HasFlag(SprinklerState.Off))
            return;

        groundStateService.IncreaseHumidity(_settings.Intensity);
    }
}