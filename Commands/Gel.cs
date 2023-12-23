using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
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
        GetPlayers()
              .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, "@t", player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      x.Teleport(player.PlayerPawn.Value.AbsOrigin, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, tüm {ChatColors.Darkred}Mahkûmları {ChatColors.White}yanına ışınladı.");
    }

    [ConsoleCommand("gelct")]
    public void GelCt(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
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
                      x.Teleport(player.PlayerPawn.Value!.AbsOrigin!, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, tüm {ChatColors.Blue}Gardiyanları {ChatColors.White}yanına ışınladı.");
    }

    [ConsoleCommand("gelall")]
    public void GelAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
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
                      x.Teleport(player.PlayerPawn.Value!.AbsOrigin!, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
        Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Admin, tüm {ChatColors.Green}herkesi {ChatColors.White}yanına ışınladı.");
    }

    #endregion Gel
}