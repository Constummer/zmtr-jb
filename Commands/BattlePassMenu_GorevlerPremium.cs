using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<ulong, int> GorevlerPremiumMenu = new();

    [ConsoleCommand("gorevlerPremium")]
    public void BattlePassGorevlerPremium(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (!Config.Additional.BattlePassPremiumActive) return;

        if (!AdminManager.PlayerHasPermissions(player, "@css/premium"))
        {
            player.PrintToChat($"{Prefix} {CC.W}Bu menu sadece {CC.M}PREMIUM{CC.W}'lara özeldir.");
            return;
        }
    }
}