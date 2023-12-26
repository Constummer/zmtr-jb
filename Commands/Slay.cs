using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
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
                Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuyu {ChatColors.LightRed}öldürdü{ChatColors.White}.");
            }
        });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefini {ChatColors.LightRed}öldürdü{ChatColors.White}.");
        }
    }

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
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}herkesi {ChatColors.LightRed}öldürdü{ChatColors.White}.");
    }

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
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}gardiyanları {ChatColors.LightRed}öldürdü{ChatColors.White}.");
    }

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
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}mahkûmları {ChatColors.LightRed}öldürdü{ChatColors.White}.");
    }

    #endregion Slay
}