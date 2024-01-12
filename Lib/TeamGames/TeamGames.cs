using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void InitializeTG(CCSPlayerController player)
    {
        TgActive = true;

        var tgMenu = new ChatMenu("Team Games Menü | Oyunlar Seçimi");

        tgMenu.AddMenuOption("Takımlı", TeamedTeamGamesMenu);
        tgMenu.AddMenuOption("Herkes Tek", SoloTeamGamesMenu);

        ChatMenus.OpenMenu(player, tgMenu);
    }
}