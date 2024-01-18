﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Burn

    private CounterStrikeSharp.API.Modules.Timers.Timer BurnTimer;

    [ConsoleCommand("burniptal")]
    [ConsoleCommand("burnsil")]
    public void OnUnBurnCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        BurnTimer?.Kill();
        BurnTimer = null;
        return;
    }

    [ConsoleCommand("burn")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> <0,1>")]
    public void OnBurnCommand(CCSPlayerController? player, CommandInfo info)
    {
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
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefini {CC.B}burnledi");

        BurnTimer = AddTimer(0.5f, () =>
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
                var randSound = Config.Sounds.BurnSounds.Skip(_random.Next(Config.Sounds.BurnSounds.Count())).FirstOrDefault();
                x.ExecuteClientCommand($"play {randSound}");
                PerformBurn(x.PlayerPawn.Value, 1);
            });
        }, Full);
    }

    private static void PerformBurn(CBasePlayerPawn pawn, int damage = 0)
    {
        if (pawn.LifeState != (int)LifeState_t.LIFE_ALIVE)
            return;

        var vel = new Vector(pawn.AbsVelocity.X, pawn.AbsVelocity.Y, pawn.AbsVelocity.Z);

        pawn.Teleport(pawn.AbsOrigin!, pawn.AbsRotation!, vel);

        if (damage <= 0)
            return;

        pawn.Health -= damage;

        if (pawn.Health <= 0)
            pawn.CommitSuicide(true, true);
    }

    private static void PerformBurn(CCSPlayerController x, int damage)
    {
        if (ValidateCallerPlayer(x, false) == false) return;

        if (x.PawnIsAlive == false)
            return;

        if (damage <= 0)
            return;

        x.PlayerPawn.Value.Health -= damage;

        if (x.PlayerPawn.Value.Health <= 0)
        {
            x.PlayerPawn.Value.CommitSuicide(true, true);
            return;
        }
        RefreshPawnTP(x);
    }

    #endregion Burn
}