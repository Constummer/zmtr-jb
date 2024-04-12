using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Git

    [ConsoleCommand("git")]
    [CommandHelper(minArgs: 1, "<hedefteki oyuncu>")]
    public void Git(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10") == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);

        var players = GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player))
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
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var x = players.FirstOrDefault();
        if (x != null)
        {
            if (x.SteamID != player.SteamID)
            {
                var playerAbs = x.PlayerPawn.Value.AbsOrigin;
                player.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), x.PlayerPawn.Value.EyeAngles, VEC_ZERO);
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.DR}{x.PlayerName} {CC.W} adlı oyuncunun yanına ışınladı.");
            }
        }
    }

    #endregion Git
}