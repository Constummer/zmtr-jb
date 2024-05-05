using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("tr")]
    [ConsoleCommand("timereward")]
    [ConsoleCommand("treward")]
    [ConsoleCommand("timer")]
    [ConsoleCommand("trlevel")]
    [ConsoleCommand("timerlevel")]
    [ConsoleCommand("trewardlevel")]
    [ConsoleCommand("timeRewardlevel")]
    public void TimeRewardMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        if (!Config.Additional.TimeRewardActive) return;

        if (TimeRewardDatas.TryGetValue(player.SteamID, out var data))
        {
            if (data.CurrentTime >= data.Time)
            {
                data.Completed = true;
                player.PrintToChat($"{Prefix} {CC.W}Time Reward - {data.Level} levelini");
                player.PrintToChat($"{Prefix} {CC.G}Tamamlamışsın. Tebrikler.");
                if (data.Level != 31)
                {
                    player.PrintToChat($"{Prefix} {CC.W}yapman gereken {data.Level + 1} level görevleri görebilirsin.");
                    TimeRewardBase.GiveReward(data, player);
                }
                goto next;
            }

            var menu = new CenterHtmlMenu($"Time Reward - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
        return;

    next:
        if (TimeRewardDatas.TryGetValue(player.SteamID, out var data2))
        {
            var menu = new CenterHtmlMenu($"Time Reward - {data2.Level} Level", this);
            data2.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}