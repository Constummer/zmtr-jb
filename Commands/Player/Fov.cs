﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("fov")]
    [ConsoleCommand("css_fov")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void Fov(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        FovAction(player, info.ArgByIndex(1));
    }

    [ConsoleCommand("fovkapat")]
    [ConsoleCommand("fovkapa")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void FovKapa(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} FOV'u kapadı.");
        LogManagerCommand(player.SteamID, info.GetCommandString);

        FovKapaAction(player);
    }

    [ConsoleCommand("fovac")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void FovAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} FOV'u açtı.");
        Config.UnrestrictedFov.Enabled = true;
    }
}