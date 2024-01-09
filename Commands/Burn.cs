using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Burn

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
        if (int.TryParse(godOneTwoStr, out var godOneTwo) == false)
        {
            player.PrintToChat($"{Prefix}{CC.W} gecersiz miktar");
            return;
        }
        if (godOneTwo <= 0)
        {
            player.PrintToChat($"{Prefix}{CC.W} en az 1 girmelisin");
            return;
        }
        var targetArgument = GetTargetArgument(target);

        AddTimer(0.3f, () =>
        {
            GetPlayers()
                       .Where(x => x.PawnIsAlive && GetTargetAction(x, target, player.PlayerName))
                       .ToList()
                       .ForEach(x =>
                       {
                           if (targetArgument == TargetForArgument.None)
                           {
                               Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuyu{CC.B} burnledi{CC.W}.");
                           }
                           PerformSlap(x.PlayerPawn.Value, 1);
                       });
            if (targetArgument != TargetForArgument.None)
            {
                Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefini {CC.B}burnledi");
            }
        }, CounterStrikeSharp.API.Modules.Timers.TimerFlags.REPEAT);
    }

    #endregion Burn
}