﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Burn

    private CounterStrikeSharp.API.Modules.Timers.Timer BurnTimer;

    [ConsoleCommand("burniptal")]
    [ConsoleCommand("burnsil")]
    public void OnUnBurnCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        BurnTimer?.Kill();
        BurnTimer = null;
        return;
    }

    [ConsoleCommand("burn")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me>")]
    public void OnBurnCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.ArgCount > 1 ? info.GetArg(1) : null;
        var godOneTwoStr = info.ArgCount > 2 ? info.GetArg(2) : null;
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(godOneTwoStr))
        {
            return;
        }
        if (int.TryParse(godOneTwoStr, out var value) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} gecersiz miktar");
            return;
        }
        if (value <= 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} en az 1 girmelisin");
            return;
        }
        var targetArgument = GetTargetArgument(target);
        var counter = 0;
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)}, {CC.G}{target} {CC.W}hedefini {CC.B}burnledi");

        BurnTimer = AddTimer(0.3f, () =>
        {
            counter++;
            if (counter > value)
            {
                BurnTimer?.Kill();
                BurnTimer = null;
                return;
            }
            GetPlayers()
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player.PlayerName))
                       .ToList()
                       .ForEach(x =>
                       {
                           PerformSlap(x.PlayerPawn.Value, 1);
                       });
        }, TimerFlags.REPEAT);
    }

    #endregion Burn
}