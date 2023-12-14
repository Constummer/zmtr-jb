﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Kredi

    [ConsoleCommand("kredi")]
    public void Kredi(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var amount = 0;
        if (player?.SteamID != null && player!.SteamID != 0)
        {
            if (PlayerMarketModels.TryGetValue(player.SteamID, out var item))
            {
                amount = item.Credit;
            }
        }
        player!.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.LightBlue}{amount} kredin var!");
    }

    #endregion Kredi
}