using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void LRPlayerDeath(CCSPlayerController attacker, CCSPlayerController victim)
    {
        if (LrActive == false)
        {
            return;
        }
        if (ActivatedLr == null)
        {
            return;
        }
        if (victim == null)
        {
            return;
        }
        if (victim?.SteamID == null || victim?.SteamID <= 0)
        {
            return;
        }
        if (attacker == null)
        {
            return;
        }
        if (attacker?.SteamID == null || attacker?.SteamID <= 0)
        {
            return;
        }
        if (ActivatedLr?.GardSteamId == null || ActivatedLr.GardSteamId <= 0)
        {
            return;
        }
        if (ActivatedLr?.MahkumSteamId == null || ActivatedLr.MahkumSteamId <= 0)
        {
            return;
        }
        if (victim?.SteamID != ActivatedLr.GardSteamId && victim?.SteamID != ActivatedLr.MahkumSteamId)
        {
            return;
        }
        if (attacker?.SteamID != ActivatedLr.GardSteamId && attacker?.SteamID != ActivatedLr.MahkumSteamId)
        {
            return;
        }
        var mahk = GetPlayers().Where(x => x.SteamID == ActivatedLr.MahkumSteamId).FirstOrDefault();
        var gard = GetPlayers().Where(x => x.SteamID == ActivatedLr.GardSteamId).FirstOrDefault();
        if (mahk == null || gard == null)
        {
            return;
        }
        if (attacker.SteamID == ActivatedLr.MahkumSteamId)
        {
            Server.PrintToChatAll($"{Prefix}{CC.G} {mahk.PlayerName} {CC.W}adlı {T_AllLower}, {CC.B}{gard.PlayerName}{CC.W} adlı {CT_LowerPrePosition} olan {CC.LY}{ActivatedLr.Text} {CC.W}LR'sini kazandı.");
        }
        else
        {
            Server.PrintToChatAll($"{Prefix}{CC.G} {gard.PlayerName} {CC.W}adlı {CT_AllLower}, {CC.B}{mahk.PlayerName}{CC.W} adlı {T_LowerPrePosition} olan {CC.LY}{ActivatedLr.Text} {CC.W}LR'sini kazandı.");
        }
        Config.UnrestrictedFov.Enabled = true;

        if (FovActivePlayers.TryGetValue(mahk.SteamID, out int mahkFov))
        {
            FovAction(mahk, mahkFov.ToString());
        }
        if (FovActivePlayers.TryGetValue(gard.SteamID, out int gardFov))
        {
            FovAction(gard, gardFov.ToString());
        }
        LrCancel();
    }
}