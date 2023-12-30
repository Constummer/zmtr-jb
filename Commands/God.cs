﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region God

    [ConsoleCommand("god", "godmode a player")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0/1>")]
    public void God(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye30"))
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount < 2) return;
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : null;
        int.TryParse(godOneTwoStr, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR]{ChatColors.White} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }

        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                        && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   switch (godOneTwo)
                   {
                       case 0:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}godunu {ChatColors.White}kaldirdi.");
                           }
                           if (ActiveGodMode.TryGetValue(x.SteamID, out _))
                           {
                               ActiveGodMode[x.SteamID] = false;
                           }
                           else
                           {
                               ActiveGodMode.TryAdd(x.SteamID, false);
                           }
                           break;

                       case 1:
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}god {ChatColors.White}verdi.");
                           }
                           if (ActiveGodMode.TryGetValue(x.SteamID, out _))
                           {
                               ActiveGodMode[x.SteamID] = true;
                           }
                           else
                           {
                               ActiveGodMode.TryAdd(x.SteamID, true);
                           }
                           break;

                       default:

                           if (ActiveGodMode.TryGetValue(x.SteamID, out var god))
                           {
                               if (god)
                               {
                                   if (targetArgument == TargetForArgument.None)
                                   {
                                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}godunu {ChatColors.White} kaldırdı.");
                                   }
                               }
                               else
                               {
                                   if (targetArgument == TargetForArgument.None)
                                   {
                                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}god {ChatColors.White}verdi.");
                                   }
                               }
                               ActiveGodMode[x.SteamID] = !god;
                           }
                           else
                           {
                               if (targetArgument == TargetForArgument.None)
                               {
                                   Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}god {ChatColors.White}verdi.");
                               }
                               ActiveGodMode.TryAdd(x.SteamID, true);
                           }
                           break;
                   }
                   RefreshPawn(x);
               });
        if (targetArgument != TargetForArgument.None)
        {
            switch (godOneTwo)
            {
                case 0:
                    Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefine {ChatColors.Blue}godunu {ChatColors.White}kaldirdi.");
                    break;

                case 1:
                    Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefine {ChatColors.Blue}god {ChatColors.White}verdi.");
                    break;
            }
        }
    }

    [ConsoleCommand("q", "godmode ct player")]
    public void Q(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode[x.SteamID] = true;
                   RefreshPawn(x);
               });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Blue}gardiyanlara {ChatColors.Green}god {ChatColors.White}verdi.");
    }

    [ConsoleCommand("qq", "remove godmode ct player")]
    public void Qq(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
               .ToList()
               .ForEach(x =>
               {
                   ActiveGodMode.Remove(x.SteamID);
                   RefreshPawn(x);
               });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Blue}gardiyanların {ChatColors.Green}godunu {ChatColors.White}kaldırdı.");
    }

    private static void GodHurtCover(EventPlayerHurt @event, CCSPlayerController player)
    {
        if (ActiveGodMode.TryGetValue(@event.Userid.SteamID, out var value))
        {
            if (value)
            {
                player.Health = 100;
                player.PlayerPawn.Value!.Health = 100;
                if (player.PawnArmor != 0)
                {
                    player.PawnArmor = 100;
                }
                if (player.PlayerPawn.Value!.ArmorValue != 0)
                {
                    player.PlayerPawn.Value!.ArmorValue = 100;
                }
            }
        }
    }

    #endregion God
}