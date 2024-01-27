using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
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
        if (player.SteamID != 76561198248447996)
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
        if (player.SteamID != 76561198248447996)
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
        if (ValidateCallerPlayer(player, false) == false)
        {
            return;
        }
        if (player.SteamID != 76561198248447996) return;

        bool isFButtonPressed = (player.Pawn.Value.MovementServices!.Buttons.ButtonStates[0] & FButtonIndex) != 0;
        if (isFButtonPressed)
        {
            //for future, mouse 1 and mouse 2 , get closer etc
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

        float x, y, z;
        x = player.PlayerPawn.Value!.AbsOrigin!.X;
        y = player.PlayerPawn.Value!.AbsOrigin!.Y;
        z = player.PlayerPawn.Value!.AbsOrigin!.Z;
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
            Closest.PlayerPawn.Value.Teleport(@new, Closest.PlayerPawn.Value.AbsRotation!, Closest.PlayerPawn.Value.AbsVelocity);
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
                closest.PlayerPawn.Value.Teleport(@new, closest.PlayerPawn.Value.AbsRotation!, closest.PlayerPawn.Value.AbsVelocity);
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

    private static bool IsPointOnTriangle(Vector start, Vector end, Vector point, double threshold)
    {
        // Check if the point is inside the triangle defined by start, end, and threshold

        // Calculate the vectors representing the edges of the triangle
        Vector edge1 = new Vector { X = end.X - start.X, Y = end.Y - start.Y, Z = end.Z - start.Z };
        Vector edge2 = new Vector { X = point.X - start.X, Y = point.Y - start.Y, Z = point.Z - start.Z };

        // Calculate the normal vector of the triangle using the cross product of the edges
        Vector normal = new Vector
        {
            X = edge1.Y * edge2.Z - edge1.Z * edge2.Y,
            Y = edge1.Z * edge2.X - edge1.X * edge2.Z,
            Z = edge1.X * edge2.Y - edge1.Y * edge2.X
        };

        // Calculate the magnitude of the normal vector
        double normalMagnitude = Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);

        // Normalize the normal vector
        normal.X /= (float)normalMagnitude;
        normal.Y /= (float)normalMagnitude;
        normal.Z /= (float)normalMagnitude;

        // Calculate the distance from the point to the plane defined by the triangle
        double distance = Math.Abs(normal.X * (point.X - start.X) + normal.Y * (point.Y - start.Y) + normal.Z * (point.Z - start.Z));

        // Check if the distance is within the specified threshold
        return distance <= threshold;
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

    private static bool IsPointOnLine(Vector start, Vector end, Vector point)
    {
        // Define the rectangle dimensions
        float upDistance = 10;
        float sideDistance = 10;
        float downDistance = 100;

        // Calculate the direction vector of the line
        float lineDirectionX = end.X - start.X;
        float lineDirectionY = end.Y - start.Y;
        float lineDirectionZ = end.Z - start.Z;

        // Calculate the unit vector of the line direction
        float length = (float)Math.Sqrt(lineDirectionX * lineDirectionX + lineDirectionY * lineDirectionY + lineDirectionZ * lineDirectionZ);
        float unitDirectionX = lineDirectionX / length;
        float unitDirectionY = lineDirectionY / length;
        float unitDirectionZ = lineDirectionZ / length;

        // Calculate vectors for the rectangle dimensions
        Vector upVector = new Vector(unitDirectionX * upDistance, unitDirectionY * upDistance, unitDirectionZ * upDistance);
        Vector sideVector = new Vector(unitDirectionY * sideDistance, -unitDirectionX * sideDistance, 0); // perpendicular to the line
        Vector downVector = new Vector(0, 0, -downDistance);

        // Calculate the corners of the rectangle
        Vector topLeft = end + upVector - sideVector;
        Vector topRight = end + upVector + sideVector;
        Vector bottomLeft = end - downVector - sideVector;
        Vector bottomRight = end - downVector + sideVector;

        // Check if the point is within the rectangle
        return IsPointInPolygon(point, new[] { topLeft, topRight, bottomRight, bottomLeft });
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

    private static bool IsPointOnLineORJINAL(Vector start, Vector end, Vector point, double threshold)
    {
        // Check if the point is on the line defined by start and end within the specified threshold

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

        // Calculate the distance between the given point and the closest point on the line
        double distance = Math.Sqrt(Math.Pow(point.X - closestPointX, 2) + Math.Pow(point.Y - closestPointY, 2) + Math.Pow(point.Z - closestPointZ, 2));

        // Check if the distance is within the specified threshold
        return distance <= threshold;
    }

    private static Vector GetEndXYZ(CCSPlayerController player)
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

        // Uzaklık
        double distance = 1000;

        // Açılara göre XYZ koordinatlarını hesapla
        double x = karakterX + distance * Math.Cos(radianA) * Math.Cos(radianB);
        double y = karakterY + distance * Math.Cos(radianA) * Math.Sin(radianB);
        double z = karakterZ + distance * Math.Sin(radianA);

        return new Vector((float)x, (float)y, (float)z);
    }

    #endregion Grab
}