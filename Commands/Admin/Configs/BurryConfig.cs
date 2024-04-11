using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using JailbreakExtras.Lib.Configs;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("BurryConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void BurryConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<BurryConfig>("BurryConfig.json");
        if (temp != null)
        {
            _Config.Burry = Config.Burry = temp;
        }
    }
}