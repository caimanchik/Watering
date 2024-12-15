using Watering.Console.Commands.Interfaces;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Settings;

namespace Watering.Console.Commands;

public class SensorSettingsCommand(IWateringClient wateringClient) : IAsyncCommand
{
    public string TriggerName => "sensor";
    public string Documentation => "Sets the sensor settings. 1 - MeasurementPeriodInSeconds";
    public async Task<bool> ExecuteAsync(params string[] args)
    {
        if (args.Length < 1 || !int.TryParse(args[0], out var secs))
            return false;
        await wateringClient.SendSettingsChange(new SensorSettings
        {
            MeasurementPeriodInSeconds = secs
        });
        return false;
    }
}