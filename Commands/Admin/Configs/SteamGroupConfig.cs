using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("SteamGroupConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void SteamGroupConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<SteamGroupConfig>("SteamGroupConfig.json");
        if (temp != null)
        {
            _Config.SteamGroup = Config.SteamGroup = temp;
        }
    }
}