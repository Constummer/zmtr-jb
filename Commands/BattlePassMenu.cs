using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("bp")]
    [ConsoleCommand("battlepass")]
    [ConsoleCommand("battlep")]
    [ConsoleCommand("bpass")]
    [ConsoleCommand("bplevel")]
    [ConsoleCommand("battlepasslevel")]
    [ConsoleCommand("battleplevel")]
    [ConsoleCommand("bpasslevel")]
    public void BattlePassMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;

        if (!Config.Additional.BattlePassActive) return;

        if (BattlePassDatas.TryGetValue(player.SteamID, out var data))
        {
            if (data.Completed)
            {
                player.PrintToChat($"{Prefix} {CC.W}Battle Pass - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                if (data.Level != 31)
                {
                    player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                    BattlePassBase.GiveReward(data, player);
                }
                goto next;
            }

            var menu = new CenterHtmlMenu($"Battle Pass - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
        return;

    next:
        if (BattlePassDatas.TryGetValue(player.SteamID, out var data2))
        {
            var menu = new CenterHtmlMenu($"Battle Pass - {data2.Level} Level", this);
            data2.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}