using Microsoft.Extensions.Hosting;
using Watering.Console.Commands.Interfaces;

namespace Watering.Console.Services.Interfaces;

public interface ICommandService : IHostedService
{
    IEnumerable<ICommandBase> GetCommands();
    bool TryGetCommand(string name, out ICommandBase command);
}