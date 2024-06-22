using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ModelConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void ModelConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<ModelConfig>("ModelConfig.json");
        if (temp != null)
        {
            _Config.Model = Config.Model = temp;
        }
    }
}