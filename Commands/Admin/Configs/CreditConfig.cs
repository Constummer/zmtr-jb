using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("CreditConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void CreditConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<CreditConfig>("CreditConfig.json");
        if (temp != null)
        {
            _Config.Credit = Config.Credit = temp;
        }
    }
}