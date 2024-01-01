using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gel

    [ConsoleCommand("gelt")]
    public void GelT(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            return;
        }
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@t", player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 50, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, tüm {ChatColors.Darkred}Mahkûmları {ChatColors.White}yanına ışınladı.");
    }

    [ConsoleCommand("gelct")]
    public void GelCt(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            return;
        }
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@ct", player!.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 50, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, tüm {ChatColors.Blue}Gardiyanları {ChatColors.White}yanına ışınladı.");
    }

    [ConsoleCommand("gelall")]
    public void GelAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            return;
        }
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@all", player!.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player!.SteamID)
                  {
                      var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                      x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 50, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, tüm {ChatColors.Green}herkesi {ChatColors.White}yanına ışınladı.");
    }

    [ConsoleCommand("gelkirmizi")]
    public void gelkirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Red);
    }

    [ConsoleCommand("gelmavi")]
    public void gelmavi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Blue);
    }

    [ConsoleCommand("gelkoyukirmizi")]
    [ConsoleCommand("gelkirmizi2")]
    public void gelkoyukirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Darkred);
    }

    [ConsoleCommand("gelyesil")]
    public void gelyesil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Green);
    }

    [ConsoleCommand("gelaciksari")]
    [ConsoleCommand("gelsari2")]
    public void gelaciksari(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.LightYellow);
    }

    [ConsoleCommand("gelacikmavi")]
    [ConsoleCommand("gelmavi2")]
    public void gelacikmavi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.LightBlue);
    }

    [ConsoleCommand("gelkoyuyesil")]
    [ConsoleCommand("gelyesil3")]
    public void gelkoyuyesil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Olive);
    }

    [ConsoleCommand("gelacikyesil")]
    [ConsoleCommand("gelyesil2")]
    public void gelacikyesil(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Lime);
    }

    [ConsoleCommand("gelacikmor")]
    [ConsoleCommand("gelmor2")]
    public void gelacikmor(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.LightPurple);
    }

    [ConsoleCommand("gelmor")]
    public void gelmor(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Purple);
    }

    [ConsoleCommand("gelgri")]
    public void gelgri(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Grey);
    }

    [ConsoleCommand("gelsari")]
    public void gelsari(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Yellow);
    }

    [ConsoleCommand("gelaltin")]
    [ConsoleCommand("gelsari3")]
    public void gelaltin(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Gold);
    }

    [ConsoleCommand("gelgumus")]
    public void gelgumus(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Silver);
    }

    [ConsoleCommand("gelkoyumavi")]
    [ConsoleCommand("gelmavi3")]
    public void gelkoyumavi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.DarkBlue);
    }

    [ConsoleCommand("gelmavigri")]
    [ConsoleCommand("gelmavi4")]
    public void gelmavi4(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.BlueGrey);
    }

    [ConsoleCommand("gelbordo")]
    public void gelbordo(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Magenta);
    }

    [ConsoleCommand("gelacikkirmizi")]
    [ConsoleCommand("gelkirmizi2")]
    public void gelacikkirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.LightRed);
    }

    [ConsoleCommand("gelturuncu")]
    public void gelturuncu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GelTeam(player, ChatColors.Orange);
    }

    private static void GelTeam(CCSPlayerController? player, char color)
    {
        if (player.PawnIsAlive == false)
        {
            return;
        }

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
                              var playerAbs = player.PlayerPawn.Value.AbsOrigin;
                              x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 100, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                          }
                      }
                  });
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, tüm {res.Msg} Takımındaki Mahkûmları {ChatColors.White}yanına ışınladı.");
        }
    }

    #endregion Gel
}