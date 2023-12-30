using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void JointeamCommandListener()
    {
        AddCommandListener("jointeam", (player, info) =>
        {
            if (info.ArgString != null)
            {
                if (ValidateCallerPlayer(player, false) == true)
                {
                    if (player.SteamID == LatestWCommandUser)
                    {
                        RemoveWarden();
                    }
                    if (info.ArgString.Contains("0")
                     || info.ArgString.Contains("1")
                     || info.ArgString.Contains("3"))
                    {
                        player.VoiceFlags |= VoiceFlags.Muted;

                        player!.ChangeTeam(CsTeam.Terrorist);
                        return HookResult.Stop;
                    }
                }
            }
            return HookResult.Continue;
        });
    }
}