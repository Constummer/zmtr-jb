using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Git

    [ConsoleCommand("git")]
    public void Git(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        var target = info.GetArg(1);

        var players = GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player.PlayerName))
              .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($"{Prefix} {CC.W}Eşleşen oyuncu bulunamadı!");
            return;
        }
        if (players.Count != 1)
        {
            player.PrintToChat($"{Prefix} {CC.W}Birden fazla oyuncu bulundu.");
            return;
        }
        var x = players.FirstOrDefault();
        if (x != null)
        {
            if (x.SteamID != player.SteamID)
            {
                var playerAbs = x.PlayerPawn.Value.AbsOrigin;
                player.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), ANGLE_ZERO, VEC_ZERO);
                Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.DR}{x.PlayerName}'in {CC.W}yanına ışınladı.");
            }
        }
    }

    #endregion Git
}