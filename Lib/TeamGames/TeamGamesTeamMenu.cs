using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void TeamedTeamGamesMenu(CCSPlayerController player, ChatMenuOption option)
    {
        TakimYapAction(2);
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
                    player.PrintToChat($"{Prefix} {CC.W}{item.Text} takımlı oyununda bir problem var, admin ile gorusmelisin.");
                    return;
                }
                ActiveTeamGamesGameBase.GameName = item.Text;
                if (ActiveTeamGamesGameBase.HasAdditionalChoices)
                {
                    ActiveTeamGamesGameBase.AdditionalChoiceMenu(player, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        if (ActiveTeamGamesGameBase == null) return;

                        BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} takımlı oyunun başlamasına 3 saniye !");
                        TgTimer = AddTimer(3.0f, () =>
                        {
                            if (ValidateCallerPlayer(player, false) == false) return;
                            if (ActiveTeamGamesGameBase == null) return;

                            TgActive = true;
                            ActiveTeamGamesGameBase.StartGame(() =>
                            {
                                if (ActiveTeamGamesGameBase.FfActive)
                                {
                                    Server.ExecuteCommand("mp_teammates_are_enemies 1");
                                }
                                TGStartSound();

                                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} takımlı oyununu başlattı.");
                            });
                        }, SOM);
                    });
                }
                else
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (ActiveTeamGamesGameBase == null) return;

                    BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} takımlı oyunun başlamasına 3 saniye !");
                    TgTimer = AddTimer(3.0f, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        if (ActiveTeamGamesGameBase == null) return;

                        TgActive = true;
                        ActiveTeamGamesGameBase.StartGame(() =>
                        {
                            if (ValidateCallerPlayer(player, false) == false) return;
                            if (ActiveTeamGamesGameBase == null) return;
                            if (ActiveTeamGamesGameBase.FfActive)
                            {
                                Server.ExecuteCommand("mp_teammates_are_enemies 1");
                            }
                            TGStartSound();

                            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} takımlı oyununu başlattı.");
                        });
                    }, SOM);
                }
            }, item.Disabled);
        }
        ChatMenus.OpenMenu(player, teamTGMenu);
    }
}