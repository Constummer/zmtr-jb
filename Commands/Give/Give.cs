using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Give

    [ConsoleCommand("give", "Silah Verir")]
    [ConsoleCommand("weapon", "Silah Verir")]
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
        var weapon = GiveHandler(info.ArgString.GetArg(1));
        if (ValidWantedWeapon(weapon) == false)
        {
            return;
        }
        var targetArgument = GetTargetArgument(target);
        GiveAction(player.PlayerName, target, weapon, targetArgument, true);
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefine {CC.B}{weapon} {CC.W}adlı silahı verdi.");
        }
    }

    private bool ValidWantedWeapon(string weapon)
    {
        return weapon?.ToLowerInvariant() switch
        {
            "breachcharge" => false,
            "bumpmine" => false,
            "knifegg" => false,
            "melee" => false,
            "tablet" => false,
            "tagrenade" => false,
            "tripwirefire" => false,
            "zone_repulsor" => false,
            _ => true
        };
    }

    private string GiveHandler(string input)
    {
        return input?.ToLowerInvariant() switch
        {
            "ak" => "ak47",
            "m4a4" => "m4a1",
            "m4a1" => "m4a1_silencer",
            "m4a1s" => "m4a1_silencer",
            "p2000" => "hkp2000",
            "xm" => "xm1014",
            "usp" => "usp_silencer",
            "usps" => "usp_silencer",
            "smoke" => "smokegrenade",
            "sg" => "smokegrenade",
            "smk" => "smokegrenade",
            "flash" => "flashbang",
            "fb" => "flashbang",
            null => "",
            _ => input
        };
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

    #endregion Give
}