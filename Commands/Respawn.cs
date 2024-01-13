using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
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
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
                   .Where(x => x.PawnIsAlive == false && GetTargetAction(x, target, player.PlayerName))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (targetArgument == TargetForArgument.None)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} revledi{CC.W}.");
                       }
                       CustomRespawn(x);
                   });
        if (targetArgument != TargetForArgument.None)
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
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }

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
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }
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
        if (OnCommandValidater(player, true, "@css/seviye6") == false)
        {
            return;
        }

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
            });
        }
        else
        {
            CustomRespawn(x);
        }
    }

    #endregion Respawn
}