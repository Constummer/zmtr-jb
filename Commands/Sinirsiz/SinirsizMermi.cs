﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private bool UnlimitedReserverAmmoActive = false;

    #region SinirsizMermi

    [ConsoleCommand("sinirsizmermiac")]
    [ConsoleCommand("smac")]
    public void SinirsizMermiAc(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W} sınırsız mermi açtı.");

        UnlimitedReserverAmmoActive = true;
    }

    #endregion SinirsizMermi
}