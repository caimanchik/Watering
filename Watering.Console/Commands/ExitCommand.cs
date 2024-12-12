using Watering.Console.Commands.Interfaces;

namespace Watering.Console.Commands;

public class ExitCommand : ICommand
{
    public string TriggerName => "exit";
    public string Documentation => "Exits the application";
    public bool Execute(params string[] args) => true;
}