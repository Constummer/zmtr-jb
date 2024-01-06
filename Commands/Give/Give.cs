using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Give

    [ConsoleCommand("give", "Silah Verir")]
    [ConsoleCommand("weapon", "Silah Verir")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <silah kisa ismi>")]
    public void Give(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/seviye9"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
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
                       Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{weapon} {CC.W}adlı silahı verdi.");
                   }
                   x.GiveNamedItem($"weapon_{weapon}");
               });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{Prefix} {CC.G}{player.PlayerName}{CC.W} adlı admin, {CC.G}{target} {CC.W}hedefine {CC.B}{weapon} {CC.W}adlı silahı verdi.");
        }
    }

    #endregion Give
}