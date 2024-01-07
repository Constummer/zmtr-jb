﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Slap

    [ConsoleCommand("css_slap")]
    [RequiresPermissions("@css/slay")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> [hasar]")]
    public void OnSlapCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        var target = info.GetArg(1);
        int damage = 0;

        if (info.ArgCount >= 2)
        {
            int.TryParse(info.GetArg(2), out damage);
        }
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (targetArgument == TargetForArgument.None)
                   {
                       Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.B}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{damage} {CC.W}slapledi.");
                   }
                   PerformSlap(player!.Pawn.Value!, damage);
               });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}{damage} {CC.W}slapledi.");
        }
    }

    private static void PerformSlap(CBasePlayerPawn pawn, int damage = 0)
    {
        if (pawn.LifeState != (int)LifeState_t.LIFE_ALIVE)
            return;

        var vel = new Vector(pawn.AbsVelocity.X, pawn.AbsVelocity.Y, pawn.AbsVelocity.Z);

        vel.X += ((_random.Next(180) + 50) * ((_random.Next(2) == 1) ? -1 : 1));
        vel.Y += ((_random.Next(180) + 50) * ((_random.Next(2) == 1) ? -1 : 1));
        vel.Z += _random.Next(200) + 100;

        pawn.Teleport(pawn.AbsOrigin!, pawn.AbsRotation!, vel);

        if (damage <= 0)
            return;

        pawn.Health -= damage;

        if (pawn.Health <= 0)
            pawn.CommitSuicide(true, true);
    }

    #endregion Slap
}