using Microsoft.Extensions.Logging;
using Watering.Core.Entites;
using Watering.Core.Services.Interfaces;
using Watering.Core.Settings;
using Watering.Core.Settings.Enums;

namespace Watering.Core.Services;

internal class WateringService : IWateringService, IPipeInfoService
{
    private readonly IControlClient _client;
    private readonly ISprinklerService _sprinklerService;
    private readonly ILogger<WateringService> _logger;

    private readonly WateringSettings _settings = new()
    {
        SprinklerMode = SprinklerMode.Auto,
        MinHumidityLevel = 1,
        MaxHumidityLevel = 10,
    };
    
    public WateringService(
        IControlClient controlClient,
        ISprinklerService sprinklerService,
        ILogger<WateringService> logger)
    {
        _client = controlClient;
        _sprinklerService = sprinklerService;
        _logger = logger;
        
        RegisterSettingsAction();
    }
    
    public void RegisterSettingsAction()
    {
        _client.RegisterSettingsChange<WateringSettings>(TryUpdateSettings);
        return;
        
        void TryUpdateSettings(WateringSettings settings)
        {
            _settings.SprinklerMode = settings.SprinklerMode;
            _settings.MinHumidityLevel = settings.MinHumidityLevel;
            _settings.MaxHumidityLevel = settings.MaxHumidityLevel;
        }
    }

    public void PipeInfo(InfoBase info)
    {
        if (_settings.SprinklerMode is SprinklerMode.Manual || info is not SensorInfo sensorInfo)
            return;
        
        if (sensorInfo.Humidity < _settings.MinHumidityLevel)
        {
            _logger.LogInformation("Влажность {Humidity} меньше минимальной влажности {MinHumidity}",
                sensorInfo.Humidity, _settings.MinHumidityLevel);
            _sprinklerService.TurnOn();
        }
        else if (sensorInfo.Humidity > _settings.MaxHumidityLevel)
        {
            _logger.LogInformation("Влажность {Humidity} больше максимальной влажности {MaxHumidity}",
                sensorInfo.Humidity, _settings.MaxHumidityLevel);
            _sprinklerService.TurnOff();
        }
    }
}