using Microsoft.Extensions.Logging;
using Watering.Core.Services.Interfaces;
using Watering.Core.Settings;
using Watering.Core.Settings.Enums;

namespace Watering.Core.Services;

internal class SprinklerService : ISprinklerService
{
    private const int WateringPeriodSeconds = 1;
    
    private readonly SprinklerSettings _settings = new();
    private readonly IControlClient _client;
    private readonly IGroundStateService _groundStateService;
    private readonly ILogger<SprinklerService> _logger;
    private PeriodicTimer? _timer;

    public SprinklerService(
        IControlClient client,
        IGroundStateService groundStateService,
        ILogger<SprinklerService> logger)
    {
        _client = client;
        _groundStateService = groundStateService;
        _logger = logger;
        
        RegisterSettingsAction();
        CreateWateringTimer();
    }
    
    public void RegisterSettingsAction()
    {
        _client.RegisterSettingsChange<SprinklerSettings>(TryUpdateSettings);
        return;

        void TryUpdateSettings(SprinklerSettings settings)
        {
            _logger.LogInformation("Обновлены настройки поливателя");
        }
    }

    public SprinklerState State { get; private set; } = SprinklerState.Off;

    public void TurnOn()
    {
        if (State is not SprinklerState.On)
            _logger.LogInformation("Поливатель включен");
        State = SprinklerState.On;
    }

    public void TurnOff()
    {
        if (State is not SprinklerState.Off)
            _logger.LogInformation("Поливатель выключен");
        State = SprinklerState.Off;
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

        _groundStateService.IncreaseHumidity(0.1f);
    }
}