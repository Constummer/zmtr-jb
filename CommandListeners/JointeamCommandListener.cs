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
                    CustomRespawnIfActive(player);

                    return HookResult.Stop;
                }
                else
                {
                    if (CTBanCheck(player) == false)
                    {
                        player.PrintToChat($"{Prefix} {CC.W} CT banýn var, geçemezsin!");
                        player!.ChangeTeam(CsTeam.Terrorist);
                    }
                    CustomRespawnIfActive(player);
                }
            }
            return HookResult.Stop;
        });
    }
}