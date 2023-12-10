using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Respawn

    [ConsoleCommand("hrespawn", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<playerismi>")]
    public void hRespawn(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        GetPlayers()
              .Where(x => (x.PlayerName?.ToLower()?.Contains(target) ?? false)
                          && x.PawnIsAlive == false)
              .ToList()
              .ForEach(x =>
              {
                  RespawnPlayer(x);
              });
    }

    [ConsoleCommand("respawn", "öldüğü yerde canlanır")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Respawn(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        GetPlayers()
              .Where(x => x.PawnIsAlive == false
                          && GetTargetAction(x, target, player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  x.Respawn();
              });
    }

    private void RespawnPlayer(CCSPlayerController x)
    {
        if (DeathLocations.TryGetValue(x.SteamID, out Vector position))
        {
            var tempX = position.X;
            var tempY = position.Y;
            var tempZ = position.Z;
            var tpPlayer = x;
            x.Respawn();
            AddTimer(0.5f, () =>
            {
                tpPlayer.PlayerPawn.Value.Teleport(new(tempX, tempY, tempZ), new(0, 0, 0), new(0, 0, 0));
                tpPlayer.Teleport(new(tempX, tempY, tempZ), new(0, 0, 0), new(0, 0, 0));
            });
        }
        else
        {
            x.Respawn();
        }
    }

    #endregion Respawn
}