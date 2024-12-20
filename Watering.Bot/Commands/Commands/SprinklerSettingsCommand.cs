using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Enums;
using Watering.Core.Entites.Settings;

namespace Watering.Bot.Commands.Commands;

internal class SprinklerSettingsCommand(IWateringClient wateringClient) : CommandBase
{
    public override string Trigger => "/sprinkler";
    public override string Description => "Sprinkler settings command. 1 - Intensity: 2 - [CanBeNull] State";
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        if (args.Length < 1 || !float.TryParse(args[0], out var intensity))
            throw new ArgumentException(ToDescription());

        var settings = new SprinklerSettings
        {
            Intensity = intensity,
        };
        if (args.Length == 2 && Enum.TryParse(args[1], out SprinklerState state))
            settings.State = state;

        await wateringClient.SendSettingsChange(settings);
        await SendMessageAsync(bot, message.Chat.Id, "Настройки поливателя обновлены");
    }
}