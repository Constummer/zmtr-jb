using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

//using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Dictionary<string, string> _citMenuDatas = new()
    {
        {"Sandalye","models/props/de_inferno/hr_i/inferno_chair/inferno_chair.vmdl" },
        {"Toplantı Masası","models/props/cs_office/table_meeting.vmdl" },
        {"Antika Masa","models/props/de_inferno/tableantique.vmdl" },
        {"Kapalı Bahçe Çiti","models/props/hr_massive/wood_fence/wood_fence_128.vmdl" },
        {"Büyük Çit","models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_gate_003a_256.vmdl" },
        {"Kapalı Büyük Duvar","models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_cover_001_256.vmdl" },
        {"Kapalı Küçük Duvar","models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_cover_001_128.vmdl" },
        {"Delikli Küçük Çit","models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_002b_128_capped.vmdl" },
        {"Delikli Büyük Çit","models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_002_256_capped.vmdl" },
        {"Yarım Odundan Çit","models/props/de_inferno/wood_fence.vmdl" },
        {"Yarım İnşaat Çiti","models/de_overpass/construction/fence/construction_fence_1.vmdl" },
    };

    [ConsoleCommand("citmenu")]
    public void CitMenu(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (AdminManager.PlayerHasPermissions(player, "@css/premium") == false && LatestWCommandUser != player!.SteamID)
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        var marketMenu = new ChatMenu($" {CC.LB}Çit Menü");
        foreach (var item in _citMenuDatas)
        {
            marketMenu.AddMenuOption(item.Key, (p, info) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                Server.PrintToChatAll($"{Prefix}{CC.DR}{item.Key} oluşturma {CC.B}{player.PlayerName} {CC.W}'a açıldı.");
                if (CitEnabledPlayers.ContainsKey(player.SteamID))
                {
                    CitEnabledPlayers[player.SteamID] = item.Value;
                }
                else
                {
                    CitEnabledPlayers.Add(player.SteamID, item.Value);
                }
                player.PrintToChat($"{Prefix}{CC.G} Ateş ettiğin yere çit oluşacak.");
                player.PrintToChat($"{Prefix}{CC.G} Kapatmak için !citkapat yaz.");
                player.PrintToChat($"{Prefix}{CC.G} Çitleri silmek çin !cittemizle yaz.");
            });
        }
        MenuManager.OpenChatMenu(player, marketMenu);
    }
}