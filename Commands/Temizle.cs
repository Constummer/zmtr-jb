﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Temizle

    [ConsoleCommand("temizle")]
    public void Temizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TemizleAction(player);
        Server.PrintToChatAll($"{AdliAdmin(Prefix)} {CC.W}Yerdeki tum silahları sildi.");
    }

    [ConsoleCommand("dt")]
    public void DisarmTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        TemizleAction(player);
        GetPlayers(CsTeam.Terrorist)
        .Where(x => x.PawnIsAlive)
        .ToList()
        .ForEach(x =>
        {
            RemoveWeapons(x, true);
        });
        Server.PrintToChatAll($"{Prefix} {CC.W}{T_PluralCamelPossesive} silahları silindi.");
        Server.PrintToChatAll($"{AdliAdmin(Prefix)} {CC.W}Yerdeki tum silahları sildi.");
    }

    private static void TemizleAction(CCSPlayerController? player)
    {
        var target = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("weapon_");

        var playerWeaponIndexes = new List<uint>();
        GetPlayers()
            .Where(x => x.PawnIsAlive)
            .ToList()
            .ForEach(x =>
            {
                playerWeaponIndexes.AddRange(PlayerWeaponIndexes(x));
            });

        foreach (var ent in target)
        {
            if (!ent.IsValid)
            {
                continue;
            }

            if (playerWeaponIndexes.Contains(ent.Index) == false)
            {
                ent.Remove();
            }
        }
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}yerdeki tüm silahları sildi.");
    }

    #endregion Temizle
}