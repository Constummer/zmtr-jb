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
                    if (RespawnAcActive)
                    {
                        AddTimer(1, () =>
                        {
                            if (ValidateCallerPlayer(player, false) == true)
                            {
                                CustomRespawn(player);
                            }
                        });
                    }

                    return HookResult.Stop;
                }
                else
                {
                    if (RespawnAcActive)
                    {
                        AddTimer(1, () =>
                        {
                            if (ValidateCallerPlayer(player, false) == true)
                            {
                                CustomRespawn(player);
                            }
                        });
                    }
                }
            }
            return HookResult.Continue;
        });
        AddCommandListener("iseli", (player, info) =>
        {
            IsEliWardenNotify();
            return HookResult.Continue;
        });

        AddCommandListener("kapilariac", (player, info) =>
        {
            IsEliWardenNotify();
            return HookResult.Continue;
        });
    }

    private static void IsEliWardenNotify()
    {
        var warden = GetWarden();
        if (warden != null)
        {
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.R} EÐER ÝSELÝ {CC.W} ise");
            warden.PrintToChat($" {CC.LR}[ZMTR] {CC.B} !rm {CC.W}veya {CC.B}!revmenu {CC.W} yazarak ölen ctleri 3 kere revleyebilirsin");
        }
    }
}