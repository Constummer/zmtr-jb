using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void InitializeTG(CCSPlayerController player)
    {
        if (ActiveTeamGamesGameBase != null)
        {
            TgActive = false;
            TgTimer?.Kill();
            TgTimer = null;
            ActiveTeamGamesGameBase?.Clear(true);
            if (ActiveTeamGamesGameBase != null)
            {
                Server.ExecuteCommand("mp_teammates_are_enemies 0");
            }
            ActiveTeamGamesGameBase = null;
        }
        var tgMenu = new ChatMenu("Team Games Menü | Oyunlar Seçimi");

        tgMenu.AddMenuOption("Takımlı", TeamedTeamGamesMenu);
        tgMenu.AddMenuOption("Herkes Tek", SoloTeamGamesMenu);

        ChatMenus.OpenMenu(player, tgMenu);
    }
}