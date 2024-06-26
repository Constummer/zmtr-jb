﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Gravity

    [ConsoleCommand("gravityac")]
    public void GravityAc(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye28, Perm_Seviye28) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        Server.ExecuteCommand("sv_gravity 150");
        Server.PrintToChatAll($"{Prefix} {CC.W}Gravity açıldı.");
    }

    [ConsoleCommand("gravitykapa")]
    [ConsoleCommand("gravitykapat")]
    public void GravityKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, Perm_Seviye28, Perm_Seviye28) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        Server.ExecuteCommand("sv_gravity 800");
        Server.PrintToChatAll($"{Prefix} {CC.W}Gravity kapandı.");
    }

    #endregion Gravity
}