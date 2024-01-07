﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private CounterStrikeSharp.API.Modules.Timers.Timer SinirsizBombaTimer = null;

    #region SinirsizBomba

    [ConsoleCommand("sinirsizbomba")]
    [ConsoleCommand("smbomba")]
    [CommandHelper(minArgs: 1, usage: "<0/1>")]
    public void SinirsizBomba(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }

        if (info.ArgCount < 2) return;
        var oneTwoStr = info.ArgCount == 2 ? info.GetArg(1) : "0";
        int.TryParse(oneTwoStr, out var oneTwo);
        if (oneTwo < 0 || oneTwo > 1)
        {
            player.PrintToChat($"{Prefix}{CC.W} 0 = kapatmak icin, 1 = acmak icin.");
            return;
        }
        SinirsizBombaTimer = GiveSinirsizCustomNade(oneTwo, SinirsizBombaTimer, "weapon_hegrenade");
    }

    private CounterStrikeSharp.API.Modules.Timers.Timer? GiveSinirsizCustomNade(int oneTwo, CounterStrikeSharp.API.Modules.Timers.Timer sinirsizGiveTimer, string itemXName, string target = null, string playerName = null)
    {
        switch (oneTwo)
        {
            case 0:
                sinirsizGiveTimer?.Kill();
                sinirsizGiveTimer = null;
                break;

            case 1:
                sinirsizGiveTimer = AddTimer(3f, () =>
                {
                    List<CCSPlayerController> players;
                    if (string.IsNullOrWhiteSpace(target))
                    {
                        players = GetPlayers()
                                 .Where(x => x.PawnIsAlive)
                                 .ToList();
                    }
                    else
                    {
                        players = GetPlayers()
                              .Where(x => x.PawnIsAlive && GetTargetAction(x, target, playerName))
                              .ToList();
                    }

                    players.ForEach(x =>
                    {
                        if (x?.PlayerPawn?.Value?.WeaponServices?.MyWeapons != null)
                        {
                            if (x.PlayerPawn.Value.WeaponServices!.MyWeapons
                            .Any(weapon => weapon.Value != null
                                    && string.IsNullOrWhiteSpace(weapon.Value.DesignerName) == false
                                    && weapon.Value.DesignerName != "[null]" &&
                                    weapon.Value.DesignerName == itemXName
                            ))
                            {
                                return;
                            }
                            else
                            {
                                x.GiveNamedItem(itemXName);
                            }
                        }
                    });
                }, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
                break;

            default:
                break;
        }
        return sinirsizGiveTimer;
    }

    #endregion SinirsizBomba
}