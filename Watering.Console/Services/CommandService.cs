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
    
    public IEnumerable<ICommandBase> GetCommands() => _commands.Values;

    public bool TryGetCommand(string name, out ICommandBase command) => _commands.TryGetValue(name, out command);

    public Task StartAsync(CancellationToken ct)
    {
        StartCommandExecutor(ct);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
    
    private async Task StartCommandExecutor(CancellationToken ct)
    {
        while (true)
        {
            System.Console.WriteLine("Введите команду");
            var input = System.Console.ReadLine();
            var parameters = input?.Split(' ');
            
            if (parameters?.Length > 1 && parameters[^1] == "--help")
            {
                if (_commands.TryGetValue("help", out var helpCommand))
                    (helpCommand as ICommand)?.Execute(parameters[0]);
                continue;
            }
            
            if (!(parameters?.Length > 0 && _commands.TryGetValue(parameters[0], out var command)))
            {
                if (_commands.TryGetValue("help", out var helpCommand))
                    (helpCommand as ICommand)?.Execute();
                continue;
            }
            
            var isOver = command switch
            {
                ICommand syncCommand => syncCommand.Execute(parameters[1..]),
                IAsyncCommand asyncCommand => await asyncCommand.ExecuteAsync(ct, parameters[1..]),
                _ => false
            };

            if (isOver)
                return;
        }
    }
}