using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

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
        LogManagerCommand(player.SteamID, info.GetCommandString);
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
        var target = info.ArgString.GetArgSkipFromLast(1);
        var godOneTwoStr = info.ArgString.GetArgLast();
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
        LogManagerCommand(player.SteamID, info.GetCommandString);
        BurnTimer?.Kill();

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
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player))
                       .ToList()
            .ForEach(x =>
            {
                //var randSound = Config.Sounds.BurnSounds.Skip(_random.Next(Config.Sounds.BurnSounds.Count())).FirstOrDefault();
                //x.ExecuteClientCommand($"play {randSound}");
                PerformBurn(x, 1);
            });
        }, Full);
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
        Utilities.SetStateChanged(x.PlayerPawn.Value, "CBaseEntity", "m_iHealth");
    }

    #endregion Burn
}