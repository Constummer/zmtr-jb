using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("UnrestrictedFovConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void UnrestrictedFovConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<UnrestrictedFovConfig>("UnrestrictedFovConfig.json");
        if (temp != null)
        {
            _Config.UnrestrictedFov = Config.UnrestrictedFov = temp;
        }
    }
}