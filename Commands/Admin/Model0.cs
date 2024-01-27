﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Diz

    [ConsoleCommand("model0")]
    [ConsoleCommand("MODEL0")]
    public void ModelZero(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                if (x.PawnIsAlive == true)
                {
                    GiveRandomSkin(x);
                }
            });
    }

    #endregion Diz
}