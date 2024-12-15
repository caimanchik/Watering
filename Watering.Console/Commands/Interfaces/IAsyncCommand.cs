namespace Watering.Console.Commands.Interfaces;

public interface IAsyncCommand : ICommandBase
{
    Task<bool> ExecuteAsync(params string[] args);
}