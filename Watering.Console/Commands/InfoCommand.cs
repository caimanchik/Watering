using Watering.Console.Commands.Interfaces;
using Watering.Domain.Ground.Services;

namespace Watering.Console.Commands;

public class InfoCommand : ICommand
{
    private readonly IGroundService _groundService;

    public InfoCommand(IGroundService groundService) => (_groundService) = (groundService);
    
    public string TriggerName => "info";
    
    public bool Execute(params string[] args)
    {
        throw new NotImplementedException();
    }
}