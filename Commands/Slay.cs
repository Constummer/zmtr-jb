using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Slay

    [ConsoleCommand("slay", "slay")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Slay(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
        .Where(x => x.PawnIsAlive
                    && GetTargetAction(x, target, player!.PlayerName))
        .ToList()
        .ForEach(x =>
        {
            var playerPawn = x.PlayerPawn.Value;
            playerPawn!.CommitSuicide(false, true);
            if (targetArgument == TargetForArgument.None)
            {
                Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.LR}öldürdü{CC.W}.");
            }
        });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.LR}öldürdü{CC.W}.");
        }
    }

    [ConsoleCommand("sall", "slay all")]
    [ConsoleCommand("slayall", "slay all")]
    public void SlayAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers()
          .ToList()
          .ForEach(x =>
          {
              var playerPawn = x.PlayerPawn.Value;
              playerPawn!.CommitSuicide(false, true);
          });
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}herkesi {CC.LR}öldürdü{CC.W}.");
    }

    [ConsoleCommand("sct", "slay ct")]
    [ConsoleCommand("slayct", "slay ct")]
    public void SlayCT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.CounterTerrorist)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}gardiyanları {CC.LR}öldürdü{CC.W}.");
    }

    [ConsoleCommand("st", "slay t")]
    [ConsoleCommand("slayt", "slay t")]
    public void SlayT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.Terrorist)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}mahkûmları {CC.LR}öldürdü{CC.W}.");
    }

    #endregion Slay
}