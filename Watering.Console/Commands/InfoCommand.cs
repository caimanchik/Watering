using Watering.Console.Commands.Interfaces;
using Watering.Core.Services.Interfaces;

namespace Watering.Console.Commands;

public class InfoCommand(IInfoService infoService) : ICommand
{ 
    public string TriggerName => "info";
    public string Documentation => "Command to get info";

    public bool Execute(params string[] args)
    {
        var info = infoService.GetAllInfo();
        System.Console.WriteLine(info);
        return false;
    }
}