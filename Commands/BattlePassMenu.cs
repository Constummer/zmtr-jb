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

        //if (!Config.Additional.BattlePassActive) return;

        if (BattlePassDatas.TryGetValue(player.SteamID, out var data))
        {
            var menu = new CenterHtmlMenu($"Battle Pass - {data.Level} Level", this);
            data.BuildLevelMenu(menu);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
        }
    }
}