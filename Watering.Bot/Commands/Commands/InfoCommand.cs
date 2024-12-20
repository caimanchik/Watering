using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Core.Services.Interfaces;

namespace Watering.Bot.Commands.Commands;

internal class InfoCommand(IInfoService infoService) : CommandBase
{
    public override string Trigger => "/info";
    public override string Description => "Command to get info";
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        var info = infoService.GetAllInfo();
        await SendMessageAsync(bot, message.Chat.Id, info.ToString());
    }
}