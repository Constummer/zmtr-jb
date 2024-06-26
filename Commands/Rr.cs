﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region RR

    [ConsoleCommand("rr")]
    [CommandHelper(0, "<saniye>")]
    public void RR(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        var target = info.ArgCount > 1 ? info.ArgString.GetArg(0) : "1";
        if (int.TryParse(target, out var value))
        {
            if (value > 120)
            {
                player.PrintToChat("Max 120 sn girebilirsin");
                return;
            }
            else
            {
                LogManagerCommand(player.SteamID, info.GetCommandString);
                Server.ExecuteCommand($"mp_restartgame {target}");
            }
        }
        else
        {
            LogManagerCommand(player.SteamID, info.GetCommandString);
            Server.ExecuteCommand($"mp_restartgame 1");
        }
    }

    #endregion RR
}