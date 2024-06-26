﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public CCSPlayerController Closest { get; private set; } = null;
    public CEnvBeam Laser { get; set; } = null;

    #region Grab

    [ConsoleCommand("grab")]
    public void Grab(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        if (GrabAllowedSteamIds.Contains(player.SteamID) == false)
        {
            player.PrintToChat("-disabled- (for now)");
            return;
        }
        else
        {
            AllowGrabForWarden(player);
        }
    }

    [ConsoleCommand("grabclear")]
    public void GrabClear(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        if (GrabAllowedSteamIds.Contains(player.SteamID) == false)
        {
            player.PrintToChat("-disabled- (for now)");
            return;
        }
        else
        {
            //Laser?.Remove();
            Closest = null;
        }
    }

    private void GrabOnTick(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false) { return; }
        if (GrabAllowedSteamIds.Contains(player.SteamID) == false) return;

        bool isFButtonPressed = (player.Pawn.Value.MovementServices!.Buttons.ButtonStates[0] & FButtonIndex) != 0;
        if (isFButtonPressed)
        {
            //TODO: mouse 1 and mouse 2 , get closer etc
            //bool isJumpPressed = (buttons & PlayerButtons.Jump) != 0;
            AllowGrabForWarden(player);
        }
        else
        {
            Closest = null;
            //Laser?.Remove();
        }
    }

    private void AllowGrabForWarden(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false
            || player.PlayerPawn.Value!.AbsOrigin == null)
        {
            return;
        }

        var playerAbs = player.PawnIsAlive == false ? player.Pawn.Value.AbsOrigin : player.PlayerPawn.Value.AbsOrigin;
        float x, y, z;
        x = playerAbs!.X;
        y = playerAbs!.Y;
        z = playerAbs!.Z;

        var start = new Vector((float)x, (float)y, (float)z);
        var end = GetEndXYZ(player);
        if (Closest != null)
        {
            if (ValidateCallerPlayer(Closest, false) == false)
            {
                return;
            }
            ActiveGodMode[Closest.SteamID] = true;
            var @new = ProjectPointOntoLine(start, end, Closest.PlayerPawn.Value.AbsOrigin);
            Closest.PlayerPawn.Value.Teleport(@new, Closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);
            Closest.Pawn.Value.Teleport(@new, Closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);
            Closest.Teleport(@new, Closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);

            var tempClosesSteamid = Closest.SteamID;
            AddTimer(1, () =>
            {
                ActiveGodMode.Remove(tempClosesSteamid);
                GetPlayers()
                .Where(x => x.SteamID == tempClosesSteamid && x.PawnIsAlive)
                .ToList()
                .ForEach(x =>
                {
                    x.TakesDamage = true;
                    x.PlayerPawn.Value.TakesDamage = true;
                    x.Pawn.Value.TakesDamage = true;
                });
            }, SOM);
        }
        else
        {
            var closest = GetClosestPlayer(start, end, 25, player.SteamID);
            if (closest != null)
            {
                ActiveGodMode[closest.SteamID] = true;
                Closest = closest;
                var @new = ProjectPointOntoLine(start, end, closest.PlayerPawn.Value.AbsOrigin);
                closest.PlayerPawn.Value.Teleport(@new, closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);
                closest.Pawn.Value.Teleport(@new, closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);
                closest.Teleport(@new, closest.PlayerPawn.Value.EyeAngles!, VEC_ZERO);
                //if (Laser != null && Laser.IsValid)
                //{
                //    Laser.EndPos.X = Closest.PlayerPawn.Value.AbsOrigin.X;
                //    Laser.EndPos.Y = Closest.PlayerPawn.Value.AbsOrigin.Y;
                //    Laser.EndPos.Z = Closest.PlayerPawn.Value.AbsOrigin.Z;
                //    Laser.Teleport(start, ANGLE_ZERO, VEC_ZERO);
                //}
                //else
                //{
                //    Laser = DrawLaser(start, Closest.PlayerPawn.Value.AbsOrigin, LaserType.Grab, false);
                //}
                var tempClosesSteamid = closest.SteamID;
                AddTimer(1, () =>
                {
                    ActiveGodMode.Remove(tempClosesSteamid);
                    GetPlayers()
                         .Where(x => x.SteamID == tempClosesSteamid && x.PawnIsAlive)
                         .ToList()
                         .ForEach(x =>
                         {
                             x.TakesDamage = true;
                             x.PlayerPawn.Value.TakesDamage = true;
                             x.Pawn.Value.TakesDamage = true;
                         });
                }, SOM);
            }
        }
    }

    private static Vector ProjectPointOntoLine2(Vector start, Vector end, Vector point)
    {
        // Calculate the direction vector of the line
        double lineDirectionX = end.X - start.X;
        double lineDirectionY = end.Y - start.Y;
        double lineDirectionZ = end.Z - start.Z;

        // Calculate the vector from start to the point
        double pointVectorX = point.X - start.X;
        double pointVectorY = point.Y - start.Y;
        double pointVectorZ = point.Z - start.Z;

        // Calculate the scalar projection of pointVector onto the lineDirection
        double scalarProjection = (pointVectorX * lineDirectionX + pointVectorY * lineDirectionY + pointVectorZ * lineDirectionZ) /
                                 (lineDirectionX * lineDirectionX + lineDirectionY * lineDirectionY + lineDirectionZ * lineDirectionZ);

        // Clamp the scalarProjection to ensure the point lies within the line segment
        scalarProjection = Math.Max(0, Math.Min(1, scalarProjection));

        // Calculate the closest point on the line to the given point
        double closestPointX = start.X + scalarProjection * lineDirectionX;
        double closestPointY = start.Y + scalarProjection * lineDirectionY;
        double closestPointZ = start.Z + scalarProjection * lineDirectionZ;

        // Return the closest point as a Vector
        return new Vector((float)closestPointX, (float)closestPointY, (float)closestPointZ);
    }

    private static Vector ProjectPointOntoLine(Vector start, Vector end, Vector point)
    {
        // Calculate the direction vector of the line
        double lineDirectionX = end.X - start.X;
        double lineDirectionY = end.Y - start.Y;
        double lineDirectionZ = end.Z - start.Z;

        // Calculate the vector from start to the point
        double pointVectorX = point.X - start.X;
        double pointVectorY = point.Y - start.Y;
        double pointVectorZ = point.Z - start.Z;

        // Calculate the scalar projection of pointVector onto the lineDirection
        double scalarProjection = (pointVectorX * lineDirectionX + pointVectorY * lineDirectionY + pointVectorZ * lineDirectionZ) /
                                 (lineDirectionX * lineDirectionX + lineDirectionY * lineDirectionY + lineDirectionZ * lineDirectionZ);

        // Calculate the closest point on the line to the given point
        double closestPointX = start.X + scalarProjection * lineDirectionX;
        double closestPointY = start.Y + scalarProjection * lineDirectionY;
        double closestPointZ = start.Z + scalarProjection * lineDirectionZ;
        // Return the closest point as a Vector
        return new Vector((float)closestPointX, (float)closestPointY, (float)closestPointZ);
    }

    private static bool GetClosestPlayer(CCSPlayerController self, CCSPlayerController target)
    {
        if (ValidateCallerPlayer(self, false) == false) return false;
        if (ValidateCallerPlayer(target, false) == false) return false;
        float x, y, z;
        x = self.PlayerPawn.Value!.AbsOrigin!.X;
        y = self.PlayerPawn.Value!.AbsOrigin!.Y;
        z = self.PlayerPawn.Value!.AbsOrigin!.Z;
        var start = new Vector((float)x, (float)y, (float)z);
        var end = GetEndXYZ(self);
        var closest = GetClosestPlayer(start, end, 25, self.SteamID);
        if (closest == null) return false;
        if (ValidateCallerPlayer(closest, false) == false) return false;

        if (target.SteamID == closest.SteamID) return true;
        return false;
    }

    private static CCSPlayerController GetClosestPlayer(Vector start, Vector end, double threshold, ulong callerSteamId)
    {
        var players = GetPlayers().Where(x => x.PawnIsAlive
                           && x.SteamID != callerSteamId).ToList();
        var closestPlayer = (CCSPlayerController)null;
        double closestDistance = double.MaxValue;

        foreach (var player in players)
        {
            //bool isOnLine = IsPointOnLine(start, end, player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin);
            bool isOnLine = IsPointOnLine(start, end, player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin, threshold);

            if (isOnLine)
            {
                double distance = CalculateDistance(player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin, start);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }
        }

        return closestPlayer;
    }

    private static double CalculateDistance(Vector p1, Vector p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
    }

    private static bool IsPointOnLine(Vector start, Vector end, Vector point, double threshold)
    {
        // Calculate the direction vector of the line
        double lineDirectionX = end.X - start.X;
        double lineDirectionY = end.Y - start.Y;
        double lineDirectionZ = end.Z - start.Z;

        // Calculate the vector from start to the point
        double pointVectorX = point.X - start.X;
        double pointVectorY = point.Y - start.Y;
        double pointVectorZ = point.Z - start.Z;

        // Calculate the scalar projection of pointVector onto the lineDirection
        double scalarProjection = (pointVectorX * lineDirectionX + pointVectorY * lineDirectionY + pointVectorZ * lineDirectionZ) /
                                 (lineDirectionX * lineDirectionX + lineDirectionY * lineDirectionY + lineDirectionZ * lineDirectionZ);

        // Check if the scalar projection is within [0, 1], meaning the point is between start and end
        if (scalarProjection >= 0 && scalarProjection <= 1)
        {
            // Calculate the closest point on the line to the given point
            double closestPointX = start.X + scalarProjection * lineDirectionX;
            double closestPointY = start.Y + scalarProjection * lineDirectionY;
            double closestPointZ = start.Z + scalarProjection * lineDirectionZ;

            // Calculate the distance between the given point and the closest point on the line
            double distance = Math.Sqrt(Math.Pow(point.X - closestPointX, 2) + Math.Pow(point.Y - closestPointY, 2) + Math.Pow(point.Z - closestPointZ, 2));

            // Check if the distance is within the specified threshold
            return distance <= threshold;
        }

        // Point is not between start and end
        return false;
    }

    private static bool IsPointInPolygon(Vector point, Vector[] polygon)
    {
        // Use a ray casting algorithm to determine if the point is inside the polygon
        int count = 0;
        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (((polygon[i].Y <= point.Y && point.Y < polygon[j].Y) || (polygon[j].Y <= point.Y && point.Y < polygon[i].Y)) &&
                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
            {
                count++;
            }
        }

        return count % 2 == 1;
    }

    private static Vector GetEndXYZ(CCSPlayerController player, double distance = 1000)
    {
        double karakterX = player.PlayerPawn.Value.AbsOrigin.X;
        double karakterY = player.PlayerPawn.Value.AbsOrigin.Y;
        double karakterZ = player.PlayerPawn.Value.AbsOrigin.Z;

        // Açı değerleri
        double angleA = -player.PlayerPawn.Value.EyeAngles.X;   // (-90, 90) arasında
        double angleB = player.PlayerPawn.Value.EyeAngles.Y; // (-180, 180) arasında

        // Açıları dereceden radyana çevir
        double radianA = (Math.PI / 180) * angleA;
        double radianB = (Math.PI / 180) * angleB;

        // Açılara göre XYZ koordinatlarını hesapla
        double x = karakterX + distance * Math.Cos(radianA) * Math.Cos(radianB);
        double y = karakterY + distance * Math.Cos(radianA) * Math.Sin(radianB);
        double z = karakterZ + distance * Math.Sin(radianA);

        return new Vector((float)x, (float)y, (float)z);
    }

    #endregion Grab
}