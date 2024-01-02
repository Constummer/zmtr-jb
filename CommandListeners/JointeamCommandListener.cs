using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void JointeamCommandListener()
    {
        AddCommandListener("jointeam", (player, info) =>
        {
            if (info.ArgString != null)
            {
                Unmuteds = Unmuteds.Where(X => X != player.SteamID).ToList();

                player.VoiceFlags |= VoiceFlags.Muted;
                if (player.SteamID == LatestWCommandUser)
                {
                    RemoveWarden();
                }
                if (info.ArgString.Contains("0")
                 || info.ArgString.Contains("1")
                 || info.ArgString.Contains("3"))
                {
                    player!.ChangeTeam(CsTeam.Terrorist);
                    return HookResult.Stop;
                }
            }
            return HookResult.Continue;
        });
    }
}