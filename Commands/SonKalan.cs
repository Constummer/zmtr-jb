﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region SonKalan

    [ConsoleCommand("sonkalan")]
    [CommandHelper(1, "<nick>")]
    public void SonKalan(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.GetArg(1);

        var players = GetPlayers()
                .Where(x => x.PlayerName.ToLower().Contains(target.ToLower()))
                .ToList();
        if (players.Count == 0)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Eşleşen oyuncu bulunamadı!");
            return;
        }
        else if (players.Count > 1)
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Eşleşen birden fazla oyuncu bulundu!");
            return;
        }

        var sk = players.FirstOrDefault();
        if (ValidateCallerPlayer(sk, false) == false)
        {
            return;
        }
        var team = sk.TeamNum;
        GetPlayers((CsTeam)team)
          .Where(x => x.PlayerName != sk.PlayerName)
          .ToList()
          .ForEach(x =>
          {
              x.CommitSuicide(false, true);
          });
    }

    #endregion SonKalan
}