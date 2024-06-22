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
        if (OnCommandValidater(player, true, Perm_Seviye10) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        if (FindSinglePlayer(player, target, out var x, (x => x.PawnIsAlive)) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
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