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
                if (ValidateCallerPlayer(player, false) == false)
                {
                    return HookResult.Continue;
                }

                if (player!.SteamID == LatestWCommandUser
                    && GetTeam(player) == CsTeam.CounterTerrorist)
                {
                    Server.PrintToConsole("marker correct");
                    Server.PrintToConsole($"{info.GetCommandString}");
                    Server.PrintToConsole($"{string.Join(",", Config.BlockedRadio.WardenAllowedRadioCommands)}");
                    Server.PrintToConsole($"{Config.BlockedRadio.WardenAllowedRadioCommands.Contains(info.GetCommandString)}");
                    if (Config.BlockedRadio.WardenAllowedRadioCommands.Contains(info.GetCommandString))
                    {
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }
}