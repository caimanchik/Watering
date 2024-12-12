using Watering.Console.Commands.Interfaces;

namespace Watering.Console.Commands;

public class ExitCommand : ICommand
{
    public string TriggerName => "exit";
    public bool Execute(params string[] args) => true;
}