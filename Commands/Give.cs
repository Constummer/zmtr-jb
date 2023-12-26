using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Give

    [ConsoleCommand("give", "Silah Verir")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <silah kisa ismi>")]
    public void Give(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.GetArg(1);
        var weapon = info.GetArg(2);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (targetArgument == TargetForArgument.None)
                   {
                       Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{x.PlayerName} {ChatColors.White}adlı oyuncuya {ChatColors.Blue}{weapon} {ChatColors.White}adlı silahı verdi.");
                   }
                   x.GiveNamedItem($"weapon_{weapon}");
               });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player.PlayerName}{ChatColors.White} adlı admin, {ChatColors.Green}{target} {ChatColors.White}hedefine {ChatColors.Blue}{weapon} {ChatColors.White}adlı silahı verdi.");
        }
    }

    #endregion Give
}