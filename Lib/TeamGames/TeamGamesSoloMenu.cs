﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void SoloTeamGamesMenu(CCSPlayerController player, ChatMenuOption option)
    {
        var soloTGMenu = new ChatMenu("Team Games Menü | Herkes Tek");

        foreach (var item in SoloTGGamesMenu)
        {
            soloTGMenu.AddMenuOption(item.Text, (p, i) =>
            {
                var @base = GetTeamGameBase(item.SoloChoice);
                if (@base == null)
                {
                    player.PrintToChat($"{Prefix} {CC.W}{item.Text} tekli oyununda bir problem var, admin ile gorusmelisin.");
                    return;
                }
                @base.StartGame(() =>
                {
                    SetRedColorForTeamGames();
                    AddTimer(3f, () =>
                    {
                        Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.W}{item.Text} tekli oyununu başlattı.");
                    });
                });
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

            Vector currentPosition = x.Pawn.Value!.CBodyComponent?.SceneNode?.AbsOrigin ?? new Vector(0, 0, 0);
            Vector currentSpeed = new Vector(0, 0, 0);
            QAngle currentRotation = x.PlayerPawn.Value.EyeAngles ?? new QAngle(0, 0, 0);
            x.PlayerPawn.Value.Teleport(currentPosition, currentRotation, currentSpeed);
        });
    }
}