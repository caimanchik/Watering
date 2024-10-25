using Watering.Console.Commands.Interfaces;
using Watering.Console.Services.Interfaces;

namespace Watering.Console.Services;

public class CommandService : ICommandService
{
    private readonly Dictionary<string, ICommandBase> _commands = new();

    public CommandService(IEnumerable<ICommandBase> commands)
    {
        foreach (var command in commands) 
            _commands[command.TriggerName] = command;
    }
    
    public void ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}