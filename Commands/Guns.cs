﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("guns")]
    [ConsoleCommand("silah")]
    [ConsoleCommand("silahlar")]
    public void FfSilahMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player.PawnIsAlive == false)
        {
            player!.PrintToChat($"{Prefix} {CC.W}Bu komutu sadece hayatta olanlar kullanabilir.");
            return;
        }

        if (FFMenuCheck == true)
        {
            var gunMenu = new ChatMenu("Silah Menu");
            WeaponMenuHelper.GetGuns(gunMenu);
            MenuManager.OpenChatMenu(player, gunMenu);
        }
        else if (IsEliMenuCheck == true)
        {
            if (GetTeam(player) == CsTeam.CounterTerrorist)
            {
                var gunMenu = new ChatMenu("Silah Menu");
                WeaponMenuHelper.GetGuns(gunMenu, hideIseli: true);
                MenuManager.OpenChatMenu(player, gunMenu);
            }
        }
        else
        {
            player!.PrintToChat($"{Prefix} {CC.W}FF veya Iseli açık olmadığı için silah menüsüne erişemezsin.");
            return;
        }
    }
}