using Telegram.Bot;
using Telegram.Bot.Types;

namespace Watering.Bot.Commands.Commands.Abstracts;

internal abstract class CommandBase
{
    public abstract string Trigger { get; }
    public abstract string Description { get; }
    public abstract Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args);
    public string ToDescription() => $" - {Trigger}: {Description}";
    protected async Task SendMessageAsync(ITelegramBotClient bot, long chatId, string message) 
        => await bot.SendMessage(chatId, message);
    
}