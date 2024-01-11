using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region God

    [ConsoleCommand("god", "godmode a player")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0/1>")]
    public void God(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye30", "@css/seviye30") == false)
        {
            return;
        }
        if (info.ArgCount < 2)
        {
            if (ActiveGodMode.TryGetValue(player.SteamID, out var val))
            {
                if (val == true)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}kendi {CC.B}godunu {CC.W}kaldirdi.");
                }
                else
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}kendine {CC.B}god {CC.W}verdi.");
                }
                ActiveGodMode[player.SteamID] = !val;
            }
            else
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}kendine {CC.B}god {CC.W}verdi.");
                ActiveGodMode.TryAdd(player.SteamID, true);
            }
        }
        else
        {
            var target = info.ArgCount > 1 ? info.GetArg(1) : null;
            var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : null;
            int.TryParse(godOneTwoStr, out var godOneTwo);
            if (godOneTwo < 0 || godOneTwo > 1)
            {
                player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
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
                                   Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}godunu {CC.W}kaldirdi.");
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
                                   Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
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
                                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}godunu {CC.W} kaldırdı.");
                                       }
                                   }
                                   else
                                   {
                                       if (targetArgument == TargetForArgument.None)
                                       {
                                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
                                       }
                                   }
                                   ActiveGodMode[x.SteamID] = !god;
                               }
                               else
                               {
                                   if (targetArgument == TargetForArgument.None)
                                   {
                                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}god {CC.W}verdi.");
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
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}godunu {CC.W}kaldirdi.");
                        break;

                    case 1:
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}god {CC.W}verdi.");
                        break;
                }
            }
        }
    }

    [ConsoleCommand("q", "godmode ct player")]
    public void Q(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye30", "@css/seviye30") == false)
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
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.B}gardiyanlara {CC.G}god {CC.W}verdi.");
    }

    [ConsoleCommand("qq", "remove godmode ct player")]
    public void Qq(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye30", "@css/seviye30") == false)
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
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.B}gardiyanların {CC.G}godunu {CC.W}kaldırdı.");
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