using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using JailbreakExtras.Lib.Configs;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ParticleConfigReload")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void ParticleConfigReload(CCSPlayerController? player, CommandInfo info)
    {
        LogManagerCommand(1, info.GetCommandString);
        var temp = ReadCustomConfigFromPath<ParticleConfig>("ParticleConfig.json");
        if (temp != null)
        {
            _Config.Particle = Config.Particle = temp;
        }
    }
}