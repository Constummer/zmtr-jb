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
        foreach (var item in MultiTGGamesMenu.OrderBy(x => x.Disabled))
        {
            teamTGMenu.AddMenuOption(item.Text, (p, i) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (ValidateCallerPlayer(p, false) == false) return;
                ActiveTeamGamesGameBase = GetTeamGameBase(item.MultiChoice);
                if (ActiveTeamGamesGameBase == null)
                {
                    player.PrintToChat($"{Prefix} {CC.W}{item.Text} takimli oyununda bir problem var, admin ile gorusmelisin.");
                    return;
                }
                ActiveTeamGamesGameBase.GameName = item.Text;
                BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} takımlı oyunun başlamasına 3 saniye !");
                TgTimer?.Kill();
                TgTimer = AddTimer(3.0f, () =>
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (ActiveTeamGamesGameBase == null) return;

                    TgActive = true;
                    ActiveTeamGamesGameBase.StartGame(() =>
                    {
                        TakimYapAction(2);
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} takımlı oyununu başlattı.");
                    });
                });
            }, item.Disabled);
        }
        ChatMenus.OpenMenu(player, teamTGMenu);
    }
}