using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Grab

    [ConsoleCommand("grab")]
    public void Grab(CCSPlayerController? player, CommandInfo info)
    {
        if (ValidateCallerPlayer(player) == false && LatestWCommandUser != player.SteamID)
        {
            return;
        }
        player.PrintToChat("-disabled- (for now)");
        //AllowGrabForWarden(player);
    }

    private void AllowGrabForWarden(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false
            || player.PlayerPawn.Value!.AbsOrigin == null)
        {
            return;
        }
        //Logger.LogInformation("af");

        float x, y, z;
        x = player.PlayerPawn.Value!.AbsOrigin!.X;
        y = player.PlayerPawn.Value!.AbsOrigin!.Y;
        z = player.PlayerPawn.Value!.AbsOrigin!.Z;
        var start = new Vector((float)x, (float)y, (float)z);
        var end = GetEndXYZ(player);

        var players = GetPlayers().Where(x => x.PawnIsAlive
                        && x.SteamID != player.SteamID).ToList();
        var closest = GetClosestPlayer(start, end, players, 100);
        if (closest != null)
        {
            ActiveGodMode[closest.SteamID] = true;
            closest.Teleport(end, closest.PlayerPawn.Value.AbsRotation!, closest.PlayerPawn.Value.AbsVelocity);
            closest.PlayerPawn.Value.Teleport(end, closest.PlayerPawn.Value.AbsRotation!, closest.PlayerPawn.Value.AbsVelocity);
            var laser = DrawLaser(start, closest.PlayerPawn.Value.AbsOrigin);
            AddTimer(1, () =>
            {
                ActiveGodMode[closest.SteamID] = false;
            });
        }

        //Vector playerPosition = player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin;
        //QAngle viewAngles = player.PlayerPawn.Value.EyeAngles;

        //if (IsPlayerCloseToTarget(player, end, player.PlayerPawn.Value!.AbsOrigin, 100))
        //{
        //    //DetachGrapple(player);
        //    continue;
        //}
        //var angleDifference = CalculateAngleDifference(new Vector(viewAngles.X, viewAngles.Y, viewAngles.Z), end - playerPosition);
        //if (angleDifference > 180.0f)
        //{
        //    //DetachGrapple(player);
        //    Console.WriteLine($"Player {player.PlayerName} looked away from the grapple target.");
        //    continue;
        //}
        //PullPlayer(player, end, playerPosition, viewAngles);

        //if (IsPlayerCloseToTarget(player, end, playerPosition, 100))
        //{
        //    //DetachGrapple(player);
        //}
        //if (laser != null)
        //{
        //    var velocity = player.PlayerPawn.Value.AbsVelocity;
        //    var res = HookThere(start, end, velocity);

        //    //player.Teleport(res.Position, player.PlayerPawn.Value.AbsRotation, res.Velocity);
        //    player.PlayerPawn.Value.Teleport(player.PlayerPawn.Value.AbsOrigin, player.PlayerPawn.Value.AbsRotation, res);
        //}
        //LasersEntry(x, y, z);
        return;
    }

    private static CCSPlayerController GetClosestPlayer(Vector start, Vector end, List<CCSPlayerController> players, double threshold)
    {
        var closestPlayer = (CCSPlayerController)null;
        double closestDistance = double.MaxValue;

        foreach (var player in players)
        {
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