﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ulong? KomutcuAdminId = null;

    #region KomutcuAdmin

    [ConsoleCommand("ka")]
    [ConsoleCommand("komutcuadmin")]
    public void KomutcuAdmin(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            player.PrintToChat($"{Prefix} {CC.B}Sadece {CC.W} Komutçu bu menüyü açabilir");
            return;
        }
        var players = GetPlayers()
        .Where(x => AdminManager.PlayerHasPermissions(x, "@css/admin1"))
        .ToList();

        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Aktif hiç admin bulunamadı");
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var kaMenu = new ChatMenu("Komutçu Admin Menü");
        kaMenu.AddMenuOption("Seçeceğin Admin Komutçu Admin Olacak!", null, true);
        players.ForEach(x =>
        {
            kaMenu.AddMenuOption(x.PlayerName, (p, t) =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                KomutcuAdminId = x.SteamID;
                x.Clan = $"{CC.P}[Komutçu Admin]";
                AddTimer(0.2f, () =>
                {
                    if (ValidateCallerPlayer(x, false) == false) return;
                    Utilities.SetStateChanged(x, "CCSPlayerController", "m_szClan");
                    Utilities.SetStateChanged(x, "CBasePlayerController", "m_iszPlayerName");
                }, SOM);
                Server.PrintToChatAll($"{Prefix} {CC.B}{x.PlayerName} {CC.P} [Komutçu Admin]{CC.W} Olarak seçildi");
            });
        });
        ChatMenus.OpenMenu(player, kaMenu);
    }

    private void CleanTagOnKomutcuAdmin()
    {
        if (KomutcuAdminId == null)
            return;
        var ka = GetPlayers().Where(x => ValidateCallerPlayer(x, false) && x.SteamID == KomutcuAdminId).FirstOrDefault();
        if (ka != null)
        {
            ka.Clan = "";
            AddTimer(0.2f, () =>
            {
                if (ValidateCallerPlayer(ka, false) == false) return;
                Utilities.SetStateChanged(ka, "CCSPlayerController", "m_szClan");
                Utilities.SetStateChanged(ka, "CBasePlayerController", "m_iszPlayerName");
            }, SOM);
        }
        KomutcuAdminId = null;
    }

    public static bool KomutcuAdminSay(CCSPlayerController? player, CommandInfo info, bool isSayTeam)
    {
        if (KomutcuAdminId != player.SteamID)
        {
            return false;
        }
        var team = GetTeam(player);
        var teamColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.LP,
            CsTeam.None => CC.Or,
        };
        var chatColor = team switch
        {
            CsTeam.CounterTerrorist => CC.BG,
            CsTeam.Terrorist => CC.Y,
            CsTeam.Spectator => CC.P,
            CsTeam.None => CC.Or,
        };
        var teamStr = team switch
        {
            CsTeam.CounterTerrorist => $"{CC.B}[{CT_AllCap}]",
            CsTeam.Terrorist => $"{CC.R}[{T_AllCap}]",
            CsTeam.Spectator => $"{CC.P}[SPEC]",
            _ => ""
        };
        var deadStr = player.PawnIsAlive == false ? $"{CC.R}*ÖLÜ*" : "";
        if (isSayTeam)
        {
            GetPlayers(team).ToList()
                .ForEach(x => x.PrintToChat($" {deadStr} {CC.M}[Komutçu Admin] {teamStr} {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}"));
        }
        else
        {
            Server.PrintToChatAll($" {deadStr} {CC.M}[Komutçu Admin] {teamColor}{player.PlayerName} {CC.W}: {chatColor}{info.GetArg(1)}");
        }
        return true;
    }

    #endregion KomutcuAdmin
}