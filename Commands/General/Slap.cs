using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CounterStrikeSharp.API.Modules.Timers.Timer? UberSlapTimer { get; set; } = null;

    #region Slap

    [ConsoleCommand("slap")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> [hasar]")]
    public void OnSlapCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10") == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        int damage = 0;

        if (info.ArgCount >= 2)
        {
            int.TryParse(info.ArgString.GetArg(1), out damage);
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player))
               .ToList()
               .ForEach(x =>
               {
                   if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.B}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{damage} {CC.W}slapledi.");
                   }
                   PerformSlap(x!.Pawn.Value!, x.PlayerPawn.Value.EyeAngles, damage);
               });
        if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}{damage} {CC.W}slapledi.");
        }
    }

    [ConsoleCommand("uberslap")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> [hasar]")]
    public void OnUberSlapCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10") == false)
        {
            return;
        }
        var target = info.ArgString.GetArg(0);
        int damage = 0;

        if (info.ArgCount >= 2)
        {
            int.TryParse(info.ArgString.GetArg(1), out damage);
        }
        var targetArgument = GetTargetArgument(target);
        var players = GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player))
               .ToList();
        LogManagerCommand(player.SteamID, info.GetCommandString);
        if (players.Count > 0)
        {
            players.ForEach(x =>
            {
                if (ValidateCallerPlayer(x, false) == false) return;
                if ((targetArgument & TargetForArgument.SingleUser) == targetArgument)
                {
                    Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.B}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{damage} {CC.W}uberslapledi.");
                }
            });
            UberSlapTimer?.Kill();

            UberSlapTimer = AddTimer(0.1f, () =>
              {
                  players.ForEach(x =>
                  {
                      if (ValidateCallerPlayer(x, false) == false) return;
                      PerformSlap(x!.Pawn.Value!, x.PlayerPawn.Value.EyeAngles, damage);
                  });
              }, Full);
            if ((targetArgument & TargetForArgument.SingleUser) != targetArgument)
            {
                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}{damage} {CC.W}slapledi.");
            }
        }
    }

    [ConsoleCommand("uberslapkapa")]
    [ConsoleCommand("uskapa")]
    [ConsoleCommand("uberslapkapat")]
    [ConsoleCommand("uberslapiptal")]
    public void OnUberSlapKapaCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye10") == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);
        UberSlapTimer?.Kill();
        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}uberslapi kapadı.");
    }

    private static void PerformSlap(CBasePlayerPawn pawn, QAngle eyeAngles, int damage = 0)
    {
        if (pawn.LifeState != (int)LifeState_t.LIFE_ALIVE)
            return;

        var vel = new Vector(pawn.AbsVelocity.X, pawn.AbsVelocity.Y, pawn.AbsVelocity.Z);

        vel.X += ((_random.Next(180) + 50) * ((_random.Next(2) == 1) ? -1 : 1));
        vel.Y += ((_random.Next(180) + 50) * ((_random.Next(2) == 1) ? -1 : 1));
        vel.Z += _random.Next(200) + 100;

        pawn.Teleport(pawn.AbsOrigin!, eyeAngles, vel);

        if (damage <= 0)
            return;

        pawn.Health -= damage;

        if (pawn.Health <= 0)
            pawn.CommitSuicide(true, true);
    }

    #endregion Slap
}