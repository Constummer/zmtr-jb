using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using System.Collections.Concurrent;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static ConcurrentDictionary<ulong, int> PlayerBPPMenus = new();

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

        if (PlayerBPPMenus.TryGetValue(player.SteamID, out var lvl))
        {
            if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data2))
            {
                if (data2.Level <= lvl)
                {
                    var menu = new CenterHtmlMenu($"Battle Pass Premium - {data2.Level} Level", this);
                    GetNextLvlOptions(menu, lvl);
                    data2.BuildLevelMenu(menu);
                    GetRewardOptions(menu, data2);
                    MenuManager.OpenCenterHtmlMenu(this, player, menu);
                    PlayerBPPMenus.AddOrUpdate(player.SteamID, lvl, (k, v) => lvl);
                }
                else
                {
                    OpenLvlMenu(player, lvl);
                }
            }
            else
            {
                OpenLvlMenu(player, lvl);
            }
        }
        else
        {
            if (BattlePassPremiumDatas.TryGetValue(player.SteamID, out var data2))
            {
                var menu = new CenterHtmlMenu($"Battle Pass Premium - {data2.Level} Level", this);
                GetNextLvlOptions(menu, data2.Level);
                data2.BuildLevelMenu(menu);
                GetRewardOptions(menu, data2);
                MenuManager.OpenCenterHtmlMenu(this, player, menu);
                PlayerBPPMenus.AddOrUpdate(player.SteamID, data2.Level, (k, v) => data2.Level);
            }
            else
            {
                OpenLvlMenu(player, 1);
            }
        }

        void GetRewardOptions(CenterHtmlMenu menu, BattlePassPremiumBase lvl)
        {
            if (lvl.TP != 0)
            {
                menu.AddMenuOption($"Ödül - {lvl.TP} TP", null, true);
            }
            if (lvl.Credit != 0)
            {
                menu.AddMenuOption($"Ödül - {lvl.Credit} Kredi", null, true);
            }
        }

        void GetNextLvlOptions(CenterHtmlMenu menu, int lvl)
        {
            menu.AddMenuOption($"Önceki Level", (a, b) =>
            {
                OpenLvlMenu(player, lvl - 1 < 0 ? 1 : lvl - 1);
            });
            menu.AddMenuOption($"Sonraki Level", (a, b) =>
            {
                OpenLvlMenu(player, lvl + 1 > 31 ? 31 : lvl + 1);
            });
        }

        void OpenLvlMenu(CCSPlayerController? player, int lvl)
        {
            var data = GetBattlePassPremiumLevelConfig(lvl);
            var menu = new CenterHtmlMenu($"Battle Pass Premium - {data.Level} Level", this);
            GetNextLvlOptions(menu, lvl);
            data.BuildLevelMenu(menu);
            GetRewardOptions(menu, data);
            MenuManager.OpenCenterHtmlMenu(this, player, menu);
            PlayerBPPMenus.AddOrUpdate(player.SteamID, lvl, (k, v) => lvl);
        }
    }
}