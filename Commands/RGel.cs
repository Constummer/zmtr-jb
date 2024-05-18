using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Disarm

    [ConsoleCommand("rgel")]
    public void RGel(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (!AdminManager.PlayerHasPermissions(player, Perm_Premium))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var revIds = new List<ulong>();
        GetPlayers()
                   .Where(x => x.PawnIsAlive == false && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} revledi{CC.W}.");
                       }
                       revIds.Add(x.SteamID);
                       CustomRespawn(x);
                   });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}revledi");
        }

        GetPlayers()
                   .Where(x => x.PawnIsAlive && revIds.Contains(x.SteamID))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x.SteamID != player.SteamID)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} yanına ışınladı{CC.W}.");
                           var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                           x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), x.PlayerPawn.Value.EyeAngles, VEC_ZERO);
                       }
                   });
    }

    #endregion Disarm
}