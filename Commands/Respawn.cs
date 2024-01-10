using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool RespawnAcActive = false;

    #region Respawn

    [ConsoleCommand("respawn")]
    [ConsoleCommand("rev")]
    [ConsoleCommand("res")]
    [ConsoleCommand("revive")]
    public void Respawn(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
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
        if (ValidateCallerPlayer(player) == false)
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
        if (ValidateCallerPlayer(player) == false)
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
        if (ValidateCallerPlayer(player) == false)
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

    [ConsoleCommand("respawnac")]
    public void RespawnAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye27"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        GetPlayers()
            .Where(x => x.PawnIsAlive == false)
            .ToList()
            .ForEach(x =>
            {
                CustomRespawn(x);
            });
        RespawnAcAction();
    }

    [ConsoleCommand("respawnkapa")]
    [ConsoleCommand("respawnkapat")]
    public void RespawnKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye27"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        RespawnKapatAction();
    }

    private static void RespawnAcAction()
    {
        Server.ExecuteCommand("mp_respawn_on_death_ct 1");
        Server.ExecuteCommand("mp_respawn_on_death_t 1");
        Server.ExecuteCommand("mp_ignore_round_win_conditions 1");
        Server.PrintToChatAll($"{Prefix} {CC.W}Respawn açıldı.");
        RespawnAcActive = true;
    }

    private static void RespawnKapatAction()
    {
        Server.ExecuteCommand("mp_respawn_on_death_ct 0");
        Server.ExecuteCommand("mp_respawn_on_death_t 0");
        Server.ExecuteCommand("mp_ignore_round_win_conditions 0");
        Server.PrintToChatAll($"{Prefix} {CC.W}Respawn kapandı.");
        RespawnAcActive = false;
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