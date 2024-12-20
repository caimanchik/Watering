using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Settings;

namespace Watering.Bot.Commands.Commands;

internal class WateringSettingsCommand(IWateringClient wateringClient) : CommandBase
{
    public override string Trigger => "/watering";
    public override string Description =>
        "Sets the watering settings. 1 - Mode; 2 - MinHumidityLevel; 3 - MaxHumidityLevel";
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException(ToDescription());

        if (!Enum.TryParse(args[0], out SprinklerMode mode)
            || !float.TryParse(args[1], out var minHumidityLevel)
            || !float.TryParse(args[2], out var maxHumidityLevel))
            throw new ArgumentException(ToDescription());

        await wateringClient.SendSettingsChange(new WateringSettings
        {
            SprinklerMode = mode,
            MinHumidityLevel = minHumidityLevel,
            MaxHumidityLevel = maxHumidityLevel,
        });

        await SendMessageAsync(bot, message.Chat.Id, "Настройки полива обновлены");
    }
}