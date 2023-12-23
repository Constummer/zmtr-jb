using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region FreeKill

    [ConsoleCommand("fk", "herkesi durdurma")]
    [ConsoleCommand("freekill", "herkesi durdurma")]
    public void FreeKill(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (GetTeam(player) != CsTeam.CounterTerrorist)
        {
            return;
        }
        if (KilledPlayers.TryGetValue(player.SteamID, out var list))
        {
            if (list != null && list.Count > 0)
            {
                var killMenu = new ChatMenu("Kill Menu");
                foreach (var item in list.Reverse())
                {
                    killMenu.AddMenuOption(item.Value, RespawnSelectedPlayer);
                }

                ChatMenus.OpenMenu(player, killMenu);
            }
        }
        else
        {
            player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.White}Zaten kimseyi öldürmedin.");
        }
    }

    private void RespawnSelectedPlayer(CCSPlayerController controller, ChatMenuOption option)
    {
        if (KilledPlayers.TryGetValue(controller.SteamID, out var list))
        {
            if (list != null && list.Count > 0)
            {
                var data = list.ToList().Where(x => x.Value == option.Text).ToList() ?? new();
                foreach (var item in data)
                {
                    var player = Utilities.GetPlayerFromSteamId(item.Key);
                    if (player != null)
                    {
                        RespawnPlayer(player);
                        player.PrintToChat($" {ChatColors.LightRed}[ZMTR] {ChatColors.Green}{player} {ChatColors.White}adlı oyuncuyu canlandırdın.");
                    }
                }
            }
        }
    }

    #endregion FreeKill
}