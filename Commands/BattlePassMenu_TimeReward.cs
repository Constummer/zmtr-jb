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
            var menu = new CenterHtmlMenu($"Time Reward - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}