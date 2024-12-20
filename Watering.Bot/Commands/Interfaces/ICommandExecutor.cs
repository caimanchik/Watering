using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;

namespace Watering.Bot.Commands.Interfaces;

internal interface ICommandExecutor
{
    Task ExecuteAsync(ITelegramBotClient bot, Message message);
    IEnumerable<CommandBase> GetCommands();
}