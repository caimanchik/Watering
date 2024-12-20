using Microsoft.Extensions.Logging;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;
using Watering.Core.Ground.Interfaces;

namespace Watering.Core.Ground;

internal class SensorService(
    IControlClient client,
    IGroundStateService groundStateService,
    ILogger<SensorService> logger)
    : ISensorService
{
    private readonly SensorSettings _settings = new()
    {
        MeasurementPeriodInSeconds = 5,
    };

    private PeriodicTimer? _timer;

    public void RegisterSettingsAction()
    {
        client.RegisterSettingsChange<SensorSettings>(TryUpdateSettings);
        return;

        Task TryUpdateSettings(SensorSettings settings)
        {
            _settings.MeasurementPeriodInSeconds = settings.MeasurementPeriodInSeconds;
            InitTimer();
            
            // logger.LogInformation(
            //     "Обновлены настройки сенсора. MeasurementPeriodInSeconds = {MeasurementPeriodInSeconds}",
            //     _settings.MeasurementPeriodInSeconds);
            
            return Task.CompletedTask;
        }
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterSettingsAction();
        InitTimer();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    private async void InitTimer()
    {
        if (_timer is null)
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.MeasurementPeriodInSeconds));
            // logger.LogInformation("Создан таймер сенсора с периодом {Period} (ms)", _timer.Period.Milliseconds);
            while (await _timer.WaitForNextTickAsync()) 
                OnTimerTick();
        }
        else
            _timer.Period = TimeSpan.FromSeconds(_settings.MeasurementPeriodInSeconds);
    }

    private void OnTimerTick()
    {
        var sensorInfo = new SensorInfo
        {
            Humidity = groundStateService.Humidity,
            MeasurementPeriodSeconds = _settings.MeasurementPeriodInSeconds,
        };
        
        // logger.LogInformation("Измерена влажность. Показатели прибора: Humidity = {Humidity}",
        //     sensorInfo.Humidity);

        client.SendInfo(sensorInfo);
    }
}