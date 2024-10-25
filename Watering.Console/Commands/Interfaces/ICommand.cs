namespace Watering.Console.Commands.Interfaces;

public interface ICommand : ICommandBase
{
    bool Execute(params string[] args);
}