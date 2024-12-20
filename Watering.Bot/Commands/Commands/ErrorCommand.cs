using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;

namespace Watering.Bot.Commands.Commands;

internal class ErrorCommand : CommandBase
{
    public override string Trigger => "/error";
    public override string Description => "Shows error with specified text";
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        await SendMessageAsync(bot, message.Chat.Id, args.Length > 0 ? args[0] : "Unknown exception");
    }
}