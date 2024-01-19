using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SoloTeamGamesMenu(CCSPlayerController player, ChatMenuOption option)
    {
        SetRedColorForTeamGames();

        if (ValidateCallerPlayer(player, false) == false) return;
        var soloTGMenu = new ChatMenu("Team Games Menü | Herkes Tek");

        foreach (var item in SoloTGGamesMenu.OrderBy(x => x.Disabled))
        {
            soloTGMenu.AddMenuOption(item.Text, (p, i) =>
            {
                if (ValidateCallerPlayer(player, false) == false) return;
                if (ValidateCallerPlayer(p, false) == false) return;
                ActiveTeamGamesGameBase = GetTeamGameBase(item.SoloChoice);
                if (ActiveTeamGamesGameBase == null)
                {
                    player.PrintToChat($"{Prefix} {CC.W}{item.Text} tekli oyununda bir problem var, admin ile gorusmelisin.");
                    return;
                }
                ActiveTeamGamesGameBase.GameName = item.Text;
                if (ActiveTeamGamesGameBase.HasAdditionalChoices)
                {
                    ActiveTeamGamesGameBase.AdditionalChoiceMenu(player, () =>
                    {
                        if (ValidateCallerPlayer(player, false) == false) return;
                        if (ActiveTeamGamesGameBase == null) return;

                        BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} tekli oyunun başlamasına 3 saniye !");
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
                                Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} tekli oyununu başlattı.");
                            });
                        }, SOM);
                    });
                }
                else
                {
                    if (ValidateCallerPlayer(player, false) == false) return;
                    if (ActiveTeamGamesGameBase == null) return;

                    BasicCountdown.CommandStartTextCountDown(this, $"{item.Text} tekli oyunun başlamasına 3 saniye !");
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
                            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} tekli oyununu başlattı.");
                        });
                    }, SOM);
                }
            }, item.Disabled);
        }
        ChatMenus.OpenMenu(player, soloTGMenu);
    }

    private static void SetRedColorForTeamGames()
    {
        var players = GetPlayers()
            .Where(x => x != null
                 && x.PlayerPawn.IsValid
                 && x.PawnIsAlive
                 && x.IsValid
                 && x?.PlayerPawn?.Value != null
                 && GetTeam(x) == CsTeam.Terrorist
                 && ValidateCallerPlayer(x, false));

        players.ToList().ForEach(x =>
        {
            SetColour(x, Color.FromArgb(255, 0, 0));

            //Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            //Vector currentSpeed = new Vector(0, 0, 0);
            //QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            //x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
            x.PrintToChat("Herkes Tek oyunu başlamak üzere.");
            x.PrintToCenter("Herkes Tek oyunu başlamak üzere.");
        });
    }
}