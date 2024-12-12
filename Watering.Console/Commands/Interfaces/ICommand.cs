namespace Watering.Console.Commands.Interfaces;

public interface ICommand : ICommandBase
{
    /// <summary>
    /// Execute command
    /// </summary>
    /// <param name="args"></param>
    /// <returns>Is exit command</returns>
    bool Execute(params string[] args);
}