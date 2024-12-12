namespace Watering.Console.Commands.Interfaces;

public interface ICommandBase
{
    string TriggerName { get; }
    string Documentation { get; }
}