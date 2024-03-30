using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region lr

    [ConsoleCommand("lriptal")]
    [ConsoleCommand("lrsil")]
    [ConsoleCommand("lrkaldir")]
    [ConsoleCommand("cancellr")]
    public void OnLrCancelCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (LrActive == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} Aktif LR bulunmuyor.");
            return;
        }
        if (ActivatedLr == null)
        {
            player.PrintToChat($"{Prefix}{CC.W} Aktif LR bulunmuyor.");
            return;
        }

        if (ActivatedLr?.GardSteamId == null || ActivatedLr.GardSteamId <= 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} Aktif LR bulunmuyor.");
            return;
        }
        if (ActivatedLr?.MahkumSteamId == null || ActivatedLr.MahkumSteamId <= 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} Aktif LR bulunmuyor.");
            return;
        }

        var mahk = GetPlayers().Where(x => x.SteamID == ActivatedLr.MahkumSteamId).FirstOrDefault();
        var gard = GetPlayers().Where(x => x.SteamID == ActivatedLr.GardSteamId).FirstOrDefault();
        if (mahk == null || gard == null)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        RemoveWeapons(mahk, true);
        RemoveWeapons(gard, true);
        SetColour(gard, DefaultColor);
        SetColour(mahk, DefaultColor);
        Server.PrintToChatAll($"{Prefix}{CC.W} LR iptal edildi.");
        LrCancel();
    }

    [ConsoleCommand("lr")]
    public void OnLrCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son {T_AllLower} lr atabilir.");
            return;
        }
        if (GetTeam(player) != CsTeam.Terrorist)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son {T_AllLower} lr atabilir.");
            return;
        }
        if (GetPlayerCount(CsTeam.Terrorist, true) != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Sadece yaşayan son {T_AllLower} lr atabilir.");
            return;
        }
        if (LrActive)
        {
            player.PrintToChat($"{Prefix} {CC.W}Mevcutta bir LR var, Tekrar açılamaz.");
            return;
        }
        HandlerLrMenus(player);
    }

    #endregion lr
}