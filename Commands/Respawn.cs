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

    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [ConsoleCommand("1up", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void hRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye8"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        switch (targetArgument)
        {
            case TargetForArgument.None:
                {
                    var ps = GetPlayers()
                            .Where(x => (x.PlayerName?.ToLower()?.Contains(target) ?? false)
                                       && x.PawnIsAlive == false)
                            .ToList();
                    if (ps.Count > 1)
                    {
                        player.PrintToChat($"{Prefix} {CC.W} Birden fazla eşleşme bulundu, eğer isim veremiyorsanız #1 gibi oyuncu id ile deneyebilirsin.");
                    }
                    ps.ForEach(x =>
                    {
                        RespawnPlayer(x);
                        if (targetArgument == TargetForArgument.None)
                        {
                            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            case TargetForArgument.Me:
                RespawnPlayer(player);
                Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{player.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");

                break;

            case TargetForArgument.UserIdIndex:
                {
                    var ps = GetPlayers()
                            .Where(x => GetUserIdIndex(target) == x.UserId
                                       && x.PawnIsAlive == false)
                            .ToList();
                    if (ps.Count > 1)
                    {
                        player.PrintToChat($"{Prefix} {CC.W} Birden fazla eşleşme bulundu, eğer isim veremiyorsanız #1 gibi oyuncu id ile deneyebilirsin.");
                    }
                    ps.ForEach(x =>
                    {
                        RespawnPlayer(x);
                        if (targetArgument == TargetForArgument.None)
                        {
                            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                        }
                    });
                }
                break;

            default:
                break;
        }
    }

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
                           Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} revledi{CC.W}.");
                       }
                       CustomRespawn(x);
                   });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}revledi");
        }
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