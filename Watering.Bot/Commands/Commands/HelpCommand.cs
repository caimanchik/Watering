using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Bot.Commands.Interfaces;

namespace Watering.Bot.Commands.Commands;

internal class HelpCommand(IServiceProvider services) : CommandBase
{
    private string? _commandsDescriptions;
    
    public override string Trigger => "/help";
    public override string Description => "Shows help with commands descriptions";
    
    public override async Task ExecuteAsync(ITelegramBotClient bot, Message message, params string[] args)
    {
        var response = GetCommandsDescriptions();
        await SendMessageAsync(bot, message.Chat.Id, response);
    }
    
    private string GetCommandsDescriptions()
    {
        if (_commandsDescriptions is not null)
            return _commandsDescriptions;

        var sb = new StringBuilder();
        var commandExecutor = services.GetRequiredService<ICommandExecutor>();

        foreach (var command in commandExecutor.GetCommands())
        {
            if (command.Trigger == "/error")
                continue;
            sb.AppendLine(command.ToDescription());
        }
        
        _commandsDescriptions = sb.ToString();
        return _commandsDescriptions;
    }
}