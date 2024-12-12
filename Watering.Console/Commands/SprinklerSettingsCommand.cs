using Watering.Console.Commands.Interfaces;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Settings;

namespace Watering.Console.Commands;

public class SprinklerSettingsCommand(IWateringClient wateringClient) : ICommand
{
    public string TriggerName => "sprinkler";
    public string Documentation => "Sprinkler settings command. 1 - Intensity: 2 - [CanBeNull] State";
    public bool Execute(params string[] args)
    {
        if (args.Length < 1 || !float.TryParse(args[0], out var intensity))
            return false;

        var settings = new SprinklerSettings
        {
            Intensity = intensity,
        };
        if (args.Length == 2 && Enum.TryParse(args[1], out SprinklerState state))
            settings.State = state;

        wateringClient.SendSettingsChange(settings);
        return false;
    }
}