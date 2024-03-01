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
                    return HookResult.Stop;
                }
                Server.PrintToConsole($"BlockRadioCommandsLoad LatestWCommandUser {LatestWCommandUser} 2");
                Server.PrintToConsole($"BlockRadioCommandsLoad SteamID {player.SteamID} 2");

                var latestW = LatestWCommandUser;
                Server.PrintToConsole($"BlockRadioCommandsLoad latestW {latestW} 2.1");
                if (latestW == null)
                {
                    latestW = GetWId();
                }
                Server.PrintToConsole($"BlockRadioCommandsLoad latestW {latestW} 2.2");
                if (latestW == null)
                {
                    return HookResult.Stop;
                }
                Server.PrintToConsole($"BlockRadioCommandsLoad latestW {latestW} 2.3");

                if (player.SteamID == latestW)
                {
                    Server.PrintToConsole($"BlockRadioCommandsLoad {info.GetCommandString} 3");
                    if (Config.BlockedRadio.WardenAllowedRadioCommands.Contains(info.GetCommandString))
                    {
                        Server.PrintToConsole($"BlockRadioCommandsLoad {info.GetCommandString} 4");
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }

    private ulong? GetWId()
    {
        var w = GetWarden();
        if (w != null)
        {
            if (ValidateCallerPlayer(w, false) == false)
            {
                return null;
            }
        }
        return w.SteamID;
    }
}