using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("SendDcNotifyOnWardenCommand")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void SendDcNotifyOnWardenCommand(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        SendDcNotifyOnWardenChange();
    }
}