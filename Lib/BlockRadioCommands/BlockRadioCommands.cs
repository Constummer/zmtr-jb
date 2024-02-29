using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public void BlockRadioCommandsLoad()
    {
        foreach (var command in Config.BlockedRadio.BlockedRadioCommands)
        {
            AddCommandListener(command, (player, info) =>
            {
                Server.PrintToConsole($"BlockRadioCommandsLoad {info.GetCommandString} 1");
                if (ValidateCallerPlayer(player, false) == false)
                {
                    return HookResult.Continue;
                }

                if (player!.SteamID == LatestWCommandUser
                    && GetTeam(player) == CsTeam.CounterTerrorist)
                {
                    Server.PrintToConsole($"BlockRadioCommandsLoad {info.GetCommandString} 2");
                    if (Config.BlockedRadio.WardenAllowedRadioCommands.Contains(info.GetCommandString))
                    {
                        Server.PrintToConsole($"BlockRadioCommandsLoad {info.GetCommandString} 3");
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }
}