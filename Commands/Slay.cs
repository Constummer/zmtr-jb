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
        var target = info.ArgString.GetArg(0);
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
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.LR}öldürdü{CC.W}.");
            }
        });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.LR}öldürdü{CC.W}.");
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
        GetPlayers(CsTeam.CounterTerrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}gardiyanları {CC.LR}öldürdü{CC.W}.");
    }

    [ConsoleCommand("slayt", "slay t")]
    public void SlayT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               var playerPawn = x.PlayerPawn.Value;
               playerPawn.CommitSuicide(false, true);
           });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}mahkûmları {CC.LR}öldürdü{CC.W}.");
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