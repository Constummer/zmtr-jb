﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Bunny

    [ConsoleCommand("bunnyac", "bunny acar")]
    [ConsoleCommand("ba", "bunny acar")]
    public void BunnyAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye29"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 1;sv_autobunnyhopping 1");
        Server.PrintToChatAll($"{Prefix} {CC.W}Bunny açıldı.");
    }

    [ConsoleCommand("bk", "bunny kapar")]
    [ConsoleCommand("bunnykapa", "bunny kapar")]
    [ConsoleCommand("bunnykapat", "bunny kapar")]
    public void BunnyKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye29"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.ExecuteCommand($"sv_enablebunnyhopping 0;sv_autobunnyhopping 0");
        Server.PrintToChatAll($"{Prefix} {CC.W}Bunny kapandı.");
    }

    #endregion Bunny
}