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
            var inputSplitted = input?.Split(" ");

            if (!(inputSplitted?.Length > 0 && _commands.TryGetValue(inputSplitted[0].ToLower(), out var command)))
                continue;

            bool isOver = command switch
            {
                ICommand syncCommand => syncCommand.Execute(inputSplitted[1..]),
                IAsyncCommand asyncCommand => await asyncCommand.ExecuteAsync(ct, inputSplitted[1..]),
                _ => false
            };

            if (isOver)
                return;
        }
    }
}