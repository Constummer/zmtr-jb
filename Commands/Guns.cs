﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

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

        if (FFMenuCheck == true)
        {
            var gunMenu = new ChatMenu("Silah Menu");
            WeaponMenuHelper.GetGuns(gunMenu);
            ChatMenus.OpenMenu(player!, gunMenu);
        }
        else if (IsEliMenuCheck == true)
        {
        }
        else
        {
            player!.PrintToChat($"{Prefix} {CC.W}FF veya Iseli açık olmadığı için silah menüsüne erişemezsin.");
            return;
        }
    }
}