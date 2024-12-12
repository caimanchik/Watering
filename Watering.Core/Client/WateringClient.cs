using System.Text.Json;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core.Client;

public class WateringClient : IWateringClient, IControlClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        TypeInfoResolver = CoreJsonContext.Default
    };
    
    private event Action<string>? OnReceive;
    
    public void RegisterSettingsChange<T>(Action<T> action) where T : SettingsBase 
        => OnReceive += GetOnReceiveAction(action);

    public void RegisterInfoChange<T>(Action<T> action) where T : InfoBase
        => OnReceive += GetOnReceiveAction(action);

    public void SendInfo(InfoBase info)
    {
        var jsonString = JsonSerializer.Serialize(info, info.GetType(), JsonOptions);
        OnReceive?.Invoke(jsonString);
    }

    public void SendSettingsChange(SettingsBase settings)
    {
        var jsonString = JsonSerializer.Serialize(settings, settings.GetType(), JsonOptions);
        OnReceive?.Invoke(jsonString);
    }

    private static Action<string> GetOnReceiveAction<T>(Action<T> action)
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