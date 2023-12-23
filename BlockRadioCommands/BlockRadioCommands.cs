using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public void BlockRadioCommandsLoad()
    {
        foreach (var command in Config.BlockedRadioCommands)
        {
            AddCommandListener(command, (player, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false)
                {
                    return HookResult.Continue;
                }

                if (player!.SteamID == LatestWCommandUser
                    && GetTeam(player) == CsTeam.CounterTerrorist)
                {
                    if (Config.WardenAllowedRadioCommands.Contains(info.GetCommandString))
                    {
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }
}