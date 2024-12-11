using Microsoft.Extensions.Logging;
using Watering.Core.Client.Interfaces;
using Watering.Core.Controllers.Interfaces;
using Watering.Core.Entites;
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
        Intensivity = 0.1f
    };
    
    private PeriodicTimer? _timer;

    public void RegisterSettingsAction()
    {
        client.RegisterSettingsChange<SprinklerSettings>(TryUpdateSettings);
        return;

        void TryUpdateSettings(SprinklerSettings settings)
        {
            _settings.Intensivity = settings.Intensivity;
            logger.LogInformation("Обновлены настройки поливателя");
        }
    }

    public SprinklerState State { get; private set; } = SprinklerState.Off;

    public void TurnOn()
    {
        if (State is not SprinklerState.On)
            logger.LogInformation("Поливатель включен");
        State = SprinklerState.On;
        SendInformation();
    }

    public void TurnOff()
    {
        if (State is not SprinklerState.Off)
            logger.LogInformation("Поливатель выключен");
        State = SprinklerState.Off;
        SendInformation();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterSettingsAction();
        CreateWateringTimer();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private void SendInformation()
    {
        client.SendInfo(new SprinklerInfo
        {
            Intensivity = _settings.Intensivity,
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
        if (State is SprinklerState.Off)
            return;

        groundStateService.IncreaseHumidity(_settings.Intensivity);
    }
}