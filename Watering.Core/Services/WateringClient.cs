using System.Text.Json;
using Watering.Core.Entites;
using Watering.Core.Services.Interfaces;
using Watering.Core.Settings;

namespace Watering.Core.Services;

public class WateringClient : IWateringClient, IControlClient, IPipeInfoService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        TypeInfoResolver = CoreJsonContext.Default
    };
    
    private event Action<string>? OnReceive;
    
    public void RegisterSettingsChange<T>(Action<T> action) where T : SettingsBase 
        => OnReceive += GetOnReceiveAction(action);

    public void SendSettingsChange(SettingsBase settings)
    {
        var jsonString = JsonSerializer.Serialize(settings, settings.GetType(), JsonOptions);
        OnReceive?.Invoke(jsonString);
    }
    
    public void PipeInfo(InfoBase info)
    {
        // todo
    }

    private static Action<string> GetOnReceiveAction<T>(Action<T> action) where T : SettingsBase
    {
        return received =>
        {
            try
            {
                var receivedSettings = JsonSerializer.Deserialize<T>(received, JsonOptions);
                if (receivedSettings is null)
                    return;

                action(receivedSettings);
            }
            catch (Exception)
            {
                // ignored
            }
        };
    }
}