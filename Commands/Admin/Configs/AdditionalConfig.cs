using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("AdditionalConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void AdditionalConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<AdditionalConfig>("AdditionalConfig.json");
        if (temp != null)
        {
            _Config.Additional = Config.Additional = temp;
            Cits = new(_Config.Additional.CitMaxCount);
        }
    }
}