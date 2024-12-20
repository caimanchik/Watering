using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Settings;

namespace Watering.Bot.Commands.Commands;

internal class SensorSettingsCommand(IWateringClient wateringClient) : CommandBase
{
    public override string Trigger => "/sensor";
    public override string Description => "Sets the sensor settings. 1 - MeasurementPeriodInSeconds";
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        if (args.Length < 1 || !int.TryParse(args[0], out var secs))
            throw new ArgumentException(ToDescription());
        
        await wateringClient.SendSettingsChange(new SensorSettings
        {
            MeasurementPeriodInSeconds = secs
        });
        await SendMessageAsync(bot, message.Chat.Id, "Настройки сенсора изменены");
    }
}