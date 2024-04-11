using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("AllConfigRefresh")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void AllConfigRefresh(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        ReadInitConfig();
    }
}