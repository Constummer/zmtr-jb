using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Give

    [ConsoleCommand("give", "Silah Verir")]
    [ConsoleCommand("silahver", "Silah Verir")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <silah kisa ismi>")]
    public void Give(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9", "@css/seviye9") == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        var weapon = info.ArgString.GetArg(1);
        var targetArgument = GetTargetArgument(target);
        GiveAction(player.PlayerName, target, weapon, targetArgument, true);
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}{weapon} {CC.W}adlı silahı verdi.");
        }
    }

    internal static void GiveAction(string playerName, string target, string weapon, TargetForArgument targetArgument, bool sendMsg)
    {
        GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, playerName))
               .ToList()
               .ForEach(x =>
               {
                   if (sendMsg)
                   {
                       if (targetArgument == TargetForArgument.None)
                       {
                           Server.PrintToChatAll($"{AdliAdmin(playerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{weapon} {CC.W}adlı silahı verdi.");
                       }
                   }
                   x.GiveNamedItem($"weapon_{weapon}");
               });
    }

    [ConsoleCommand("weapon", "Silah Verir")]
    [CommandHelper(2, "<oyuncu ismi,@t,@ct,@all,@me> <silah kisa ismi>")]
    public void GiveWeapon(CCSPlayerController? player, CommandInfo info)
    {
        if (OnCommandValidater(player, true, "@css/seviye9", "@css/seviye9") == false)
        {
            return;
        }
        if (info.ArgCount != 3) return;
        var target = info.ArgString.GetArg(0);
        var weapon = info.ArgString.GetArg(1);
        var targetArgument = GetTargetArgument(target);
        GetPlayers()
               .Where(x => x.PawnIsAlive
                          && GetTargetAction(x, target, player.PlayerName))
               .ToList()
               .ForEach(x =>
               {
                   if (targetArgument == TargetForArgument.None)
                   {
                       Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya {CC.B}{weapon} {CC.W}adlı silahı verdi.");
                   }
                   x.GiveNamedItem($"weapon_{weapon}");
               });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}{weapon} {CC.W}adlı silahı verdi.");
        }
    }

    #endregion Give
}