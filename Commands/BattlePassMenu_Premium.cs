using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("bpp")]
    [ConsoleCommand("battlepassp")]
    [ConsoleCommand("battlepp")]
    [ConsoleCommand("bpassp")]
    [ConsoleCommand("bpplevel")]
    [ConsoleCommand("battlepassplevel")]
    [ConsoleCommand("battlepplevel")]
    [ConsoleCommand("bpassplevel")]
    [ConsoleCommand("bppremium")]
    [ConsoleCommand("battlepasspremium")]
    [ConsoleCommand("battleppremium")]
    [ConsoleCommand("bpasspremium")]
    [ConsoleCommand("bppremiumlevel")]
    [ConsoleCommand("battlepasspremiumlevel")]
    [ConsoleCommand("battleppremiumlevel")]
    [ConsoleCommand("bpasspremiumlevel")]
    public void BattlePassPremiumMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (!Config.Additional.BattlePassPremiumActive) return;

        if (!AdminManager.PlayerHasPermissions(player, "@css/premium"))
        {
            player.PrintToChat($"{Prefix} {CC.W}Bu menu sadece {CC.M}PREMIUM{CC.W}'lara özeldir.");
            return;
        }

        if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data))
        {
            var menu = new CenterHtmlMenu($"Battle Pass Premium - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}