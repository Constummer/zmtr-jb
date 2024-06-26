﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region CT AL

    [ConsoleCommand("ctal", "T de yasayan herkesi ct alir")]
    public void CTal(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers(CsTeam.Terrorist)
           .Where(x => x.PawnIsAlive)
           .ToList()
           .ForEach(x =>
           {
               if (ValidateCallerPlayer(x, false) == false) return;
               if (CTBanCheck(x) == false)
               {
                   Server.PrintToChatAll($"{Prefix} {CC.W}{x.PlayerName} CT banı olduğu için CT atılamadı!");
               }
               else
               {
                   x.PlayerPawn.Value!.CommitSuicide(false, true);
                   x!.ChangeTeam(CsTeam.CounterTerrorist);
               }
           });
        SlayAllAction();

        Server.PrintToChatAll($"{Prefix} {CC.W}Yaşayan {T_PluralCamel}, {CT_CamelCase} takımına atıldı.");
    }

    #endregion CT AL
}