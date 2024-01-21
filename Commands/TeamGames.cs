﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tgdurdur")]
    [ConsoleCommand("tgiptal")]
    [ConsoleCommand("tgcancel")]
    [ConsoleCommand("tgboz")]
    [ConsoleCommand("boztg")]
    [ConsoleCommand("iptaltg")]
    [ConsoleCommand("canceltg")]
    [ConsoleCommand("durdurtg")]
    [ConsoleCommand("durdurteamgames")]
    [ConsoleCommand("teamgamesdurdur")]
    public void OnTeamGamesCancelCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        TgActive = false;
        TgTimer?.Kill();
        TgTimer = null;
        ActiveTeamGamesGameBase?.Clear(true);
        if (ActiveTeamGamesGameBase != null)
        {
            Server.ExecuteCommand("mp_teammates_are_enemies 0");
        }
        ActiveTeamGamesGameBase = null;
    }

    [ConsoleCommand("teamgames")]
    [ConsoleCommand("tg")]
    public void OnTeamGamesCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (LatestWCommandUser != player.SteamID)
        {
            if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
            {
                player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
                return;
            }
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        InitializeTG(player);
    }
}