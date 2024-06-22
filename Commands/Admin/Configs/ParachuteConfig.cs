using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using JailbreakExtras.Lib.Configs;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ParachuteConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void ParachuteConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<ParachuteConfig>("ParachuteConfig.json");
        if (temp != null)
        {
            _Config.Parachute = Config.Parachute = temp;
        }
    }
}