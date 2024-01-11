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
            Server.PrintToChatAll($"{Prefix}{CC.G}{mahk.PlayerName}{CC.W}adlı mahkûm,{CC.B}{gard.PlayerName}{CC.W} adlı gardiyanla olan {CC.LY}{ActivatedLr.Text} {CC.W}LR'sini kazandı.");
        }
        else
        {
            Server.PrintToChatAll($"{Prefix}{CC.G}{gard.PlayerName}{CC.W}adlı gardiyan,{CC.B}{mahk.PlayerName}{CC.W} adlı mahkûmla olan {CC.LY}{ActivatedLr.Text} {CC.W}LR'sini kazandı.");
        }
        LrCancel();
    }
}