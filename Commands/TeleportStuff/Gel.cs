using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gel

    [ConsoleCommand("gel")]
    public void Gel(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);
        var targetArgument = GetTargetArgument(target);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
                   .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                   .ToList()
                   .ForEach(x =>
                   {
                       if (x.SteamID != player.SteamID)
                       {
                           if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                           {
                               Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} yanına ışınladı{CC.W}.");
                           }
                           var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                           x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), ANGLE_ZERO, VEC_ZERO);
                       }
                   });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}yanına ışınladı");
        }
    }

    [ConsoleCommand("gelt")]
    public void GelT(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@t", player))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.DR}{T_PluralLowerObjective} {CC.W}yanına ışınladı.");
    }

    [ConsoleCommand("gelct")]
    public void GelCt(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@ct", player))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.B}{CT_PluralLowerObjective} {CC.W}yanına ışınladı.");
    }

    [ConsoleCommand("gelall")]
    public void GelAll(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9") == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@all", player))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {CC.G}herkesi {CC.W}yanına ışınladı.");
    }

    #endregion Gel
}