using Watering.Console.Commands.Interfaces;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Settings;

namespace Watering.Console.Commands;

public class WateringSettingsCommand(IWateringClient wateringClient) : IAsyncCommand
{
    public string TriggerName => "watering";
    public string Documentation => "Sets the watering settings. 1 - Mode; 2 - MinHumidityLevel; 3 - MaxHumidityLevel";
    public async Task<bool> ExecuteAsync(params string[] args)
    {
        if (args.Length < 3)
            return false;

        if (!Enum.TryParse(args[0], out SprinklerMode mode)
            || !float.TryParse(args[1], out var minHumidityLevel)
            || !float.TryParse(args[2], out var maxHumidityLevel))
            return false;

        await wateringClient.SendSettingsChange(new WateringSettings
        {
            SprinklerMode = mode,
            MinHumidityLevel = minHumidityLevel,
            MaxHumidityLevel = maxHumidityLevel,
        });
        
        return false;
    }
}