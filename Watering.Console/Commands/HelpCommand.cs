using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Watering.Console.Commands.Interfaces;
using Watering.Console.Services.Interfaces;

namespace Watering.Console.Commands;

public class HelpCommand(IServiceProvider serviceProvider) : ICommand
{
    public string TriggerName => "help";
    public string Documentation => "Shows help. 0 arguments to get help on registered command or type --help after registered command";
    
    public bool Execute(params string[] args)
    {
        var commandService = serviceProvider.GetRequiredService<ICommandService>();
        if (args.Length > 0 && commandService.TryGetCommand(args[0], out var commandForHelp))
        {
            System.Console.WriteLine($" - {commandForHelp.TriggerName}: {commandForHelp.Documentation}");
            return false;
        }

        var sb = new StringBuilder();

        foreach (var command in commandService.GetCommands()) 
            sb.AppendLine($" - {command.TriggerName}: {command.Documentation}");
        
        System.Console.WriteLine(sb.ToString());
        
        return false;
    }
}