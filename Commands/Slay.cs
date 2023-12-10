using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API;

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

        GetPlayers()
        .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target, player.PlayerName))
        .ToList()
        .ForEach(x =>
        {
            var playerPawn = x.PlayerPawn.Value;
            playerPawn.CommitSuicide(false, true);
        });
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
              playerPawn.CommitSuicide(false, true);
          });
        Server.PrintToChatAll("Öldünüz");
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
        Server.PrintToChatAll("Öldünüz");
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
        Server.PrintToChatAll("Öldünüz");
    }

    #endregion Slay
}