using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool RespawnAcActive = false;

    #region Respawn

    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [ConsoleCommand("1up", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void hRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye8"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
              .Where(x => (x.PlayerName?.ToLower()?.Contains(target) ?? false)
                          && x.PawnIsAlive == false)
              .ToList()
              .ForEach(x =>
              {
                  RespawnPlayer(x);
                  if (targetArgument == TargetForArgument.None)
                  {
                      Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu öldüğü yerde {CC.B}canlandırdı{CC.W}.");
                  }
              });
    }

    [ConsoleCommand("respawnac")]
    public void RespawnAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye27"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("mp_respawn_on_death_ct 1");
        Server.ExecuteCommand("mp_respawn_on_death_t 1");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Respawn açıldı.");
        RespawnAcActive = true;
    }

    [ConsoleCommand("respawnkapa")]
    [ConsoleCommand("respawnkapat")]
    public void RespawnKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye27"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand("mp_respawn_on_death_ct 0");
        Server.ExecuteCommand("mp_respawn_on_death_t 0");
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.W}Respawn kapandı.");
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