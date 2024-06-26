﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Disarm

    [ConsoleCommand("disarm", "Bicak dahil silme")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void Disarm(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgString.GetArgSkip(0);

        LogManagerCommand(player.SteamID, info.GetCommandString);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
        .Where(x => x.PawnIsAlive
                   && GetTargetAction(x, target, player))
        .ToList()
        .ForEach(x =>
        {
            if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu {CC.LR}disarmladı{CC.W}.");
            }

            RemoveWeapons(x, true);
        });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.LR}disarmladı{CC.W}.");
        }
    }

    [ConsoleCommand("disarmt", "Bicak dahil silme")]
    public void DisarmT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($"{Prefix} {CC.W}{T_PluralCamelPossesive} silahları silindi.");
    }

    [ConsoleCommand("disarmct", "Bicak dahil silme")]
    public void DisarmCT(CCSPlayerController? player, CommandInfo info)
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
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($"{Prefix} {CC.W}{CT_PluralCamelPossesive} silahları silindi.");
    }

    [ConsoleCommand("disarmall", "Bicak dahil silme")]
    public void DisarmAll(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        LogManagerCommand(player.SteamID, info.GetCommandString);
        GetPlayers()
        .Where(x => x.PawnIsAlive)
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($"{Prefix} {CC.W}Herkesin silahları silindi.");
    }

    #endregion Disarm
}