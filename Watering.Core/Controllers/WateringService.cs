using Microsoft.Extensions.Logging;
using Watering.Core.Client.Interfaces;
using Watering.Core.Controllers.Interfaces;
using Watering.Core.Entites;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core.Controllers;

internal class WateringService(
    IControlClient controlClient,
    ISprinklerService sprinklerService,
    ILogger<WateringService> logger)
    : IWateringService
{
    private readonly WateringSettings _settings = new()
    {
        SprinklerMode = SprinklerMode.Auto,
        MinHumidityLevel = 1,
        MaxHumidityLevel = 10,
    };

    public void RegisterSettingsAction()
    {
        controlClient.RegisterSettingsChange<WateringSettings>(TryUpdateSettings);
        return;
        
        void TryUpdateSettings(WateringSettings settings)
        {
            _settings.SprinklerMode = settings.SprinklerMode;
            _settings.MinHumidityLevel = settings.MinHumidityLevel;
            _settings.MaxHumidityLevel = settings.MaxHumidityLevel;
            
            controlClient.SendInfo(new WateringInfo
            {
                SprinklerMode = _settings.SprinklerMode,
                MinHumidityLevel = _settings.MinHumidityLevel,
                MaxHumidityLevel = _settings.MaxHumidityLevel,
            });
        }
    }

    public void RegisterInfoAction() => controlClient.RegisterInfoChange<SensorInfo>(HandleInfoChange);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterSettingsAction();
        RegisterInfoAction();
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private void HandleInfoChange(SensorInfo sensorInfo)
    {
        if (_settings.SprinklerMode is SprinklerMode.Manual)
            return;
        
        if (sensorInfo.Humidity < _settings.MinHumidityLevel)
        {
            logger.LogInformation("Влажность {Humidity} меньше минимальной влажности {MinHumidity}",
                sensorInfo.Humidity, _settings.MinHumidityLevel);
            sprinklerService.TurnOn();
        }
        else if (sensorInfo.Humidity > _settings.MaxHumidityLevel)
        {
            logger.LogInformation("Влажность {Humidity} больше максимальной влажности {MaxHumidity}",
                sensorInfo.Humidity, _settings.MaxHumidityLevel);
            sprinklerService.TurnOff();
        }
    }
}