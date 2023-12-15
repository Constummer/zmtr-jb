using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly string[] BlockedRadioCommands = new string[] { "coverme", "takepoint", "holdpos", "regroup", "followme", "takingfire", "go", "fallback", "sticktog", "getinpos", "stormfront", "report", "roger", "enemyspot", "needbackup", "sectorclear", "inposition", "reportingin", "getout", "negative", "enemydown", "compliment", "thanks", "cheer", "go_a", "go_b", "sorry", "needrop", "playerradio", "playerchatwheel", "player_ping", "chatwheel_ping" };

    private static readonly string[] WardenAllowedRadioCommands = new string[]
    {
            "player_ping"
    };

    public void BlockRadioCommandsLoad()
    {
        foreach (var command in BlockedRadioCommands)
        {
            AddCommandListener(command, (player, info) =>
            {
                if (!player.IsValid)
                {
                    return HookResult.Continue;
                }

                if (!player.PlayerPawn.IsValid)
                {
                    return HookResult.Continue;
                }

                if (WardenAllowedRadioCommands.Contains(info.GetCommandString))
                {
                    if (player.SteamID == LatestWCommandUser)
                    {
                        return HookResult.Continue;
                    }
                }

                return HookResult.Stop;
            });
        }
    }
}