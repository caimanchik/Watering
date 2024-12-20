using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Bot.Commands.Interfaces;

namespace Watering.Bot.Commands;

internal class CommandExecutor : ICommandExecutor
{
    private readonly Dictionary<string, CommandBase> _commands = new();
    
    public CommandExecutor(IEnumerable<CommandBase> commands)
    {
        foreach (var command in commands)
            _commands[command.Trigger] = command;
    }
    
    public async Task ExecuteAsync(ITelegramBotClient bot, Message message)
    {
        try
        {
            if (message.Text is not null && _commands.TryGetValue(message.Text, out var command))
            {
                await command.ExecuteAsync(bot, message);
                return;
            }
            
            if (_commands.TryGetValue("/help", out var helpCommand)) 
                await helpCommand.ExecuteAsync(bot, message);
        }
        catch (Exception e)
        {
            if (_commands.TryGetValue("/error", out var errorCommand))
                await errorCommand.ExecuteAsync(bot, message, e.Message);
        }
    }

    public IEnumerable<CommandBase> GetCommands() => _commands.Values;
}