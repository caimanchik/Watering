using Watering.Console.Commands.Interfaces;

namespace Watering.Console.Commands;

public class InfoCommand : ICommand
{ 
    public string TriggerName => "info";
    
    public bool Execute(params string[] args)
    {
        throw new NotImplementedException();
    }
}