namespace Watering.Console.Commands.Interfaces;

public interface IAsyncCommand : ICommandBase
{
    Task<bool> ExecuteAsync(CancellationToken ct, params string[] args);
}