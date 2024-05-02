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

        if (ValidateCallerPlayer(player, false) == false) return;

        if (!Config.Additional.BattlePassActive) return;

        if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data))
        {
            if (data.Completed)
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass Premium - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                if (data.Level != 21)
                {
                    player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                    BattlePassPremiumBase.GiveReward(data, player);
                }
                goto next;
            }

            var menu = new CenterHtmlMenu($"Battle Pass Premium - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
        return;

    next:

        if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data2))
        {
            var menu = new CenterHtmlMenu($"Battle Pass Premium - {data2.Level} Level", this);
            data2.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}