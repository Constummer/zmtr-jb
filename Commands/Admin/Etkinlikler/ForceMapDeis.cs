using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    [ConsoleCommand("ForceMapDeis")]
    public void ForceMapDeis(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, Perm_Root))
        {
            player.PrintToChat(NotEnoughPermission);
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        LogManagerCommand(player.SteamID, info.GetCommandString);

        var mapNames = Config.Map.MapWorkshopIds.Keys.ToList();
        var marketMenu = new ChatMenu($"Maps");
        foreach (var item in Config.Map.MapWorkshopIds)
        {
            marketMenu.AddMenuOption(item.Key, (p, i) => Server.ExecuteCommand($"host_workshop_map {item.Value}"));
        }

        MenuManager.OpenChatMenu(player, marketMenu);
    }
}