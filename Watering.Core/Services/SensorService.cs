using Microsoft.Extensions.Logging;
using Watering.Core.Entites;
using Watering.Core.Services.Interfaces;
using Watering.Core.Settings;

namespace Watering.Core.Services;

internal class SensorService : ISensorService
{
    private readonly SensorSettings _settings = new()
    {
        MeasurementPeriodSeconds = 5,
    };

    private readonly IControlClient _client;
    private readonly IGroundStateService _groundStateService;
    private readonly IPipeInfoService[] _pipeInfoServices;
    private readonly ILogger<SensorService> _logger;

    private PeriodicTimer? _timer;

    public SensorService(
        IControlClient client,
        IGroundStateService groundStateService,
        IEnumerable<IPipeInfoService> pipeInfoServices,
        ILogger<SensorService> logger)
    {
        _client = client;
        _groundStateService = groundStateService;
        _pipeInfoServices = pipeInfoServices.ToArray();
        _logger = logger;
        
        RegisterSettingsAction();
        InitTimer();
    }
    
    public void RegisterSettingsAction()
    {
        _client.RegisterSettingsChange<SensorSettings>(TryUpdateSettings);
        return;

        void TryUpdateSettings(SensorSettings settings)
        {
            _settings.MeasurementPeriodSeconds = settings.MeasurementPeriodSeconds;
            InitTimer();
            _logger.LogInformation(
                "Обновлены настройки сенсора. MeasurementPeriodSeconds = {MeasurementPeriodSeconds}",
                _settings.MeasurementPeriodSeconds);
        }
    }

    private async void InitTimer()
    {
        if (_timer is null)
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(_settings.MeasurementPeriodSeconds));
            _logger.LogInformation("Создан таймер сенсора с периодом {Period} (ms)", _timer.Period.Milliseconds);
            while (await _timer.WaitForNextTickAsync()) 
                OnTimerTick();
        }
        else
            _timer.Period = TimeSpan.FromSeconds(_settings.MeasurementPeriodSeconds);
    }

    private void OnTimerTick()
    {
        var sensorInfo = new SensorInfo
        {
            Humidity = _groundStateService.Humidity,
        };
        
        _logger.LogInformation("Измерена влажность. Показатели прибора: Humidity = {Humidity}",
            sensorInfo.Humidity);

        foreach (var service in _pipeInfoServices) 
            service.PipeInfo(sensorInfo);
    }
}