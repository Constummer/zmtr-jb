﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Respawn

    [ConsoleCommand("respawn")]
    [ConsoleCommand("rev")]
    [ConsoleCommand("res")]
    [ConsoleCommand("revive")]
    public void Respawn(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye6) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
                   .Where(x => x.PawnIsAlive == false && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} revledi{CC.W}.");
                       }
                       CustomRespawn(x);
                   });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}revledi");
        }
    }

    [ConsoleCommand("respawnt")]
    [ConsoleCommand("revt")]
    [ConsoleCommand("rest")]
    [ConsoleCommand("revivet")]
    public void RespawnT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye6) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers(CsTeam.Terrorist)
                   .Where(x => x.PawnIsAlive == false)
                   .ToList()
                   .ForEach(x =>
                   {
                       CustomRespawn(x);
                   });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@t {CC.W}hedefini {CC.B}revledi");
    }

    [ConsoleCommand("respawnct")]
    [ConsoleCommand("revct")]
    [ConsoleCommand("resct")]
    [ConsoleCommand("revivect")]
    public void RespawnCt(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye6) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.CounterTerrorist)
                   .Where(x => x.PawnIsAlive == false)
                   .ToList()
                   .ForEach(x =>
                   {
                       CustomRespawn(x);
                   });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@ct {CC.W}hedefini {CC.B}revledi");
    }

    [ConsoleCommand("respawnall")]
    [ConsoleCommand("revall")]
    [ConsoleCommand("resall")]
    [ConsoleCommand("reviveall")]
    public void RespawnAll(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye6) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        GetPlayers()
                   .Where(x => x.PawnIsAlive == false)
                   .ToList()
                   .ForEach(x =>
                   {
                       CustomRespawn(x);
                   });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}@all {CC.W}hedefini {CC.B}revledi");
    }

    private void RespawnPlayer(CCSPlayerController x)
    {
        if (DeathLocations.TryGetValue(x.SteamID, out Vector? position))
        {
            var tempX = position.X;
            var tempY = position.Y;
            var tempZ = position.Z;
            var tpPlayer = x;
            CustomRespawn(x);
            AddTimer(0.5f, () =>
            {
                if (ValidateCallerPlayer(tpPlayer, false) == false) return;
                tpPlayer.PlayerPawn.Value!.Teleport(new(tempX, tempY, tempZ), new(0, 0, 0), new(0, 0, 0));
                tpPlayer.Teleport(new(tempX, tempY, tempZ), new(0, 0, 0), new(0, 0, 0));
            }, SOM);
        }
        else
        {
            CustomRespawn(x);
        }
    }

    #endregion Respawn
}