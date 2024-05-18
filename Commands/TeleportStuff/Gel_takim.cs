using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gel

    [ConsoleCommand("gelkirmizi")]
    public void gelkirmizi(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye9) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GelTeam(player, CC.R);
    }

    [ConsoleCommand("gelmavi")]
    public void gelmavi(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye9) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GelTeam(player, CC.B);
    }

    [ConsoleCommand("gelyesil")]
    public void gelyesil(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye9) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GelTeam(player, CC.G);
    }

    [ConsoleCommand("gelgri")]
    public void gelgri(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye9) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GelTeam(player, CC.Gr);
    }

    private static void GelTeam(CCSPlayerController? player, char color)
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
                              var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                              x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                          }
                      }
                  });
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {res.Msg} takımındaki {T_PluralLowerObjective} {CC.W}yanına ışınladı.");
        }
    }

    private void GelTeam(CCSPlayerController? player, int index)
    {
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
                              var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
                              x.PlayerPawn.Value.Teleport(new Vector(playerAbs.X, playerAbs.Y + 1, playerAbs.Z), new QAngle(0f, 0f, 0f), new Vector(0f, 0f, 0f));
                          }
                      }
                  });
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} tüm {res.Msg} takımındaki {T_PluralLowerObjective} {CC.W}yanına ışınladı.");
        }
    }

    #endregion Gel
}