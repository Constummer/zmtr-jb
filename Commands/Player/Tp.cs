﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Tp

    private static bool TPActive = false;

    [ConsoleCommand("tp")]
    [ConsoleCommand("thirdperson")]
    [ConsoleCommand("thirdp")]
    public void Tp(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        player.PrintToChat($"{Prefix} {CC.W} Seviyene bakmak için  {CC.DR}!seviye {CC.W}veya{CC.DR} !seviyem {CC.W}yazabilirsin!");
        if (TPActive)
        {
            //OnTPCommand(player);
            SmoothThirdPerson(player);
        }
    }

    [ConsoleCommand("tp2")]
    public void Tp2(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        SmoothThirdPerson2(player);
    }

    [ConsoleCommand("TpActive")]
    public void TpActive(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        TPActive = !TPActive;
    }

    #endregion Tp
}