using CounterStrikeSharp.API;
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
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount < 2) return;
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : null;
        int.TryParse(godOneTwoStr, out var godOneTwo);
        if (godOneTwo < 0 || godOneTwo > 1)
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
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
                               Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}godunu {CC.W}kaldirdi.");
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
                               Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
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
                                       Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}godunu {CC.W} kaldırdı.");
                                   }
                               }
                               else
                               {
                                   if (targetArgument == TargetForArgument.None)
                                   {
                                       Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
                                   }
                               }
                               ActiveGodMode[x.SteamID] = !god;
                           }
                           else
                           {
                               if (targetArgument == TargetForArgument.None)
                               {
                                   Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
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
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}godunu {CC.W}kaldirdi.");
                    break;

                case 1:
                    Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}god {CC.W}verdi.");
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
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.B}gardiyanlara {CC.G}god {CC.W}verdi.");
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
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.B}gardiyanların {CC.G}godunu {CC.W}kaldırdı.");
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