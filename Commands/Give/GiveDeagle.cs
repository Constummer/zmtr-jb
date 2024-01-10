﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region GD

    [ConsoleCommand("gdeagle", "deagle Verir")]
    public void GiveDeagle(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye9"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        GetPlayers()
               .Where(x => x.PawnIsAlive)
               .ToList()
               .ForEach(x =>
               {
                   x.GiveNamedItem($"weapon_deagle");
               });
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}@all {CC.W}hedefine {CC.B}deagle {CC.W}adlı silahı verdi.");
    }

    #endregion GD
}