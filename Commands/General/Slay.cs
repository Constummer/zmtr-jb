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
        var target = info.ArgString.GetArgSkip(0);
        var targetArgument = GetTargetArgument(target);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
        .Where(x => x.PawnIsAlive
                    && GetTargetAction(x, target, player))
        .ToList()
        .ForEach(x =>
        {
            var playerPawn = x.PlayerPawn.Value;
            playerPawn!.CommitSuicide(false, true);
            if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.LR}öldürdü{CC.W}.");
            }
        });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.LR}öldürdü{CC.W}.");
        }
    }

    [ConsoleCommand("skirmizi", "slay")]
    [ConsoleCommand("slaykirmizi", "slay")]
    public void SlayKirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        SlayTakim(player, CC.R);
    }

    [ConsoleCommand("smavi", "slay")]
    [ConsoleCommand("slaymavi", "slay")]
    public void SlayMavi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        SlayTakim(player, CC.B);
    }

    [ConsoleCommand("syesil", "slay")]
    [ConsoleCommand("slayyesil", "slay")]
    public void SlayYesil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        SlayTakim(player, CC.G);
    }

    [ConsoleCommand("sgri", "slay")]
    [ConsoleCommand("slaygri", "slay")]
    public void SlayGri(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        SlayTakim(player, CC.Gr);
    }

    private static void SlayTakim(CCSPlayerController? player, char color)
    {
        var index = GetTeamIndexByColor(color);

        if (index == -1)
        {
            return;
        }

        if (TeamSteamIds.TryGetValue(index, out var plist))
        {
            var res = GetTeamColorAndTextByIndex(index);
            GetPlayers(CsTeam.Terrorist)
                  .Where(x => x.PawnIsAlive)
                  .ToList()
                  .ForEach(x =>
                  {
                      if (plist != null)
                      {
                          if (plist.Contains(x.SteamID))
                          {
                              x!.CommitSuicide(false, true);
                          }
                      }
                  });
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {res.Msg} takımındaki {T_PluralLowerObjective} {CC.W}öldürdü.");
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
        LogManagerCommand(player.SteamID, info.GetCommandString);
        SlayAllAction();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}herkesi {CC.LR}öldürdü{CC.W}.");
    }

    [ConsoleCommand("sct", "slay ct")]
    [ConsoleCommand("slayct", "slay ct")]
    public void SlayCT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.CounterTerrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{CT_PluralLowerObjective} {CC.LR}öldürdü{CC.W}.");
    }

    [ConsoleCommand("slayt", "slay t")]
    public void SlayT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{T_PluralLowerObjective} {CC.LR}öldürdü{CC.W}.");
    }

    private static void SlayAllAction()
    {
        GetPlayers()
          .Where(x => x.PawnIsAlive)
          .ToList()
          .ForEach(x =>
          {
              var playerPawn = x.PlayerPawn.Value;
              playerPawn!.CommitSuicide(false, true);
          });
    }

    #endregion Slay
}