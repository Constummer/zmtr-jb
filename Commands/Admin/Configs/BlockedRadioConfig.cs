using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using JailbreakExtras.Lib.Configs;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("BlockedRadioConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void BlockedRadioConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<BlockedRadioConfig>("BlockedRadioConfig.json");
        if (temp != null)
        {
            _Config.BlockedRadio = Config.BlockedRadio = temp;
        }
    }
}