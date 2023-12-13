using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gel

    [ConsoleCommand("gel")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Gel(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);
        GetPlayers()
              .Where(x => x.PawnIsAlive == false
                          && GetTargetAction(x, target, player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      x.Teleport(player.PlayerPawn.Value.AbsOrigin, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
    }

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
                          && GetTargetAction(x, "@ct", player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      x.Teleport(player.PlayerPawn.Value.AbsOrigin, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
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
                          && GetTargetAction(x, "@all", player.PlayerName))
              .ToList()
              .ForEach(x =>
              {
                  if (x.SteamID != player.SteamID)
                  {
                      x.Teleport(player.PlayerPawn.Value.AbsOrigin, new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                  }
              });
    }

    #endregion Gel
}