﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region TDaireOlustur

    [ConsoleCommand("tdaireolustur")]
    public void TDaireOlustur(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($" {CC.LR}[ZMTR]{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        GetTDairePoints(player);
        Server.PrintToChatAll($" {CC.LR}[ZMTR] {CC.G}{player.PlayerName}{CC.W} adlı admin, tüm {CC.G}@t'yi {CC.W} daire biçiminde ışınladı.");
    }

    private void GetTDairePoints(CCSPlayerController? player)
    {
        float middleX = player.PlayerPawn.Value.AbsOrigin.X;
        float middleY = player.PlayerPawn.Value.AbsOrigin.Y;
        var players = GetPlayers(CsTeam.Terrorist).Where(x => x.PawnIsAlive);
        float z = player.PlayerPawn.Value.AbsOrigin.Z;
        int numberOfPoints = players.Count();

        //// Calculate maximum radius based on the number of points
        //int maxRadius = 100;

        //// Calculate angle between each point
        //double angleIncrement = 2 * Math.PI / numberOfPoints;

        //// Calculate coordinates for each point with adjusted radius
        //for (int i = 0; i < numberOfPoints; i++)
        //{
        //    // Scale the radius based on the range from 1 to 64
        //    double radiusScale = (double)(i + 1) / numberOfPoints;
        //    int radius = (int)(maxRadius * radiusScale);

        //    double angle = i * angleIncrement;
        //    int x = (int)(middleX + radius * Math.Cos(angle));
        //    int y = (int)(middleY + radius * Math.Sin(angle));

        //    // Print or use x, y for each player
        //    Console.WriteLine($"Player {i + 1}: X={x}, Y={y}");
        //}

        //// Calculate maximum radius based on the number of points
        //int maxRadius = 100;

        //// Calculate angle between each point
        //double angleIncrement = 2 * Math.PI / numberOfPoints;

        //// Calculate coordinates for each point with adjusted radius
        //for (int i = 0; i < numberOfPoints; i++)
        //{
        //    // Scale the radius based on the range from 1 to numberOfPoints
        //    double radiusScale = (double)(i + 1) / numberOfPoints;
        //    int radius = (int)(maxRadius * radiusScale);

        //    double angle = i * angleIncrement;
        //    int x = (int)(middleX + radius * Math.Cos(angle));
        //    int y = (int)(middleY + radius * Math.Sin(angle));

        //    // Print or use x, y for each player
        //    Console.WriteLine($"Player {i + 1}: X={x}, Y={y}");
        //}
        //// Calculate maximum radius based on the number of points
        //int maxRadius = 100;

        //// Calculate angle between each point
        //double angleIncrement = 2 * Math.PI / numberOfPoints;

        //// Calculate coordinates for each point with adjusted radius
        //for (int i = 0; i < numberOfPoints; i++)
        //{
        //    // Scale the radius based on the range from 1 to numberOfPoints
        //    double radiusScale = (double)i / numberOfPoints;
        //    int radius = (int)(maxRadius * radiusScale);

        //    double angle = i * angleIncrement;
        //    int x = (int)(middleX + radius * Math.Cos(angle));
        //    int y = (int)(middleY + radius * Math.Sin(angle));

        //    // Print or use x, y for each player
        //    Console.WriteLine($"Player {i + 1}: X={x}, Y={y}");
        //}
        // Calculate maximum radius based on the number of points
        int maxRadius = 100;

        // Calculate angle between each point
        double angleIncrement = 2 * Math.PI / numberOfPoints;

        // Calculate coordinates for each point with adjusted radius
        int i = 0;
        foreach (var p in players)
        {
            double angle = i * angleIncrement;
            int x = (int)(middleX + maxRadius * Math.Cos(angle));
            int y = (int)(middleY + maxRadius * Math.Sin(angle));

            p.PlayerPawn.Value.Teleport(new Vector(x, y, z), ANGLE_ZERO, VEC_ZERO);

            i++;
        }
    }

    #endregion TDaireOlustur
}