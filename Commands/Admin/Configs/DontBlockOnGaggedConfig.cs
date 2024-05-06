using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("DontBlockOnGaggedConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void DontBlockOnGaggedConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<DontBlockOnGaggedConfig>("DontBlockOnGaggedConfig.json");
        if (temp != null)
        {
            _Config.DontBlockOnGagged = Config.DontBlockOnGagged = temp;
        }
    }
}