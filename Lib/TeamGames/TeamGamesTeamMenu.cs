using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamedTeamGamesMenu(CCSPlayerController player, ChatMenuOption option)
    {
        if (ValidateCallerPlayer(player, false) == false) return;
        var teamTGMenu = new ChatMenu("Team Games Menü | Takımlı");
        foreach (var item in MultiTGGamesMenu)
        {
            teamTGMenu.AddMenuOption(item.Text, (p, i) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (ValidateCallerPlayer(p, false) == false) return;
                var @base = GetTeamGameBase(item.MultiChoice);
                if (@base == null)
                {
                    player.PrintToChat($"{Prefix} {CC.W}{item.Text} takimli oyununda bir problem var, admin ile gorusmelisin.");
                    return;
                }
                @base.StartGame(() =>
                {
                    TakimYapAction(2);
                    AddTimer(3f, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} takimlı oyununu başlattı.");
                    });
                });
            }, item.Disabled);
        }
        ChatMenus.OpenMenu(player, teamTGMenu);
    }
}