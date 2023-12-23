using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private void ListenersOnTick()
    {
        RegisterListener((Listeners.OnTick)(() =>
        {
            bool changed = false;
            for (int i = 1; i < Server.MaxPlayers; i++)
            {
                var ent = NativeAPI.GetEntityFromIndex(i);
                if (ent == 0)
                    continue;

                var player = new CCSPlayerController(ent);
                if (player == null || !player.IsValid)
                    continue;
                //foreach (var c in player.Pawn.Value!.MovementServices!.Buttons.ButtonStates)
                //{
                //    if (c == 0)
                //        continue;
                //    Logger.LogInformation(c.ToString());
                //}
                //AllowGrabForWarden(player);
                //if (HookActive == false)
                //{
                //    AllowLaserForWarden(player);
                //}

                ParachuteOnTick(player);

                changed = BasicCountdown.CountdownEnableTextHandler(changed, player);

                //Logger.LogInformation("A={0},{1},{2}", player.AbsOrigin.X, player.AbsOrigin.Y, player.AbsOrigin.Z);
                //Logger.LogInformation("B={0},{1},{2}", player.AbsRotation.X, player.AbsRotation.Y, player.AbsRotation.Z);
                //Logger.LogInformation("C={0},{1},{2}", player.AbsVelocity.X, player.AbsVelocity.Y, player.AbsVelocity.Z);
                //Logger.LogInformation("D={0},{1},{2}", player.PlayerPawn.Value.AbsOrigin.X, player.PlayerPawn.Value.AbsOrigin.Y, player.PlayerPawn.Value.AbsOrigin.Z);
                //Logger.LogInformation("E={0},{1},{2}", player.PlayerPawn.Value.AbsRotation.X, player.PlayerPawn.Value.AbsRotation.Y, player.PlayerPawn.Value.AbsRotation.Z);
                //Logger.LogInformation("F={0},{1},{2}", player.PlayerPawn.Value.AbsVelocity.X, player.PlayerPawn.Value.AbsVelocity.Y, player.PlayerPawn.Value.AbsVelocity.Z);
                //            Logger.LogInformation("A={0},{1},{2} B={3},{4},{5} C={6},{7},{8} D={9},{10},{11} E={12},{13},{14} F={15},{16},{17}  G={18},{19},{20} H={21},{22},{23}",
                ////player.AbsOrigin.X, player.AbsOrigin.Y, player.AbsOrigin.Z,
                ////player.AbsRotation.X, player.AbsRotation.Y, player.AbsRotation.Z,
                //player.AbsVelocity.X, player.AbsVelocity.Y, player.AbsVelocity.Z,
                //player.PlayerPawn.Value.AbsOrigin.X, player.PlayerPawn.Value.AbsOrigin.Y, player.PlayerPawn.Value.AbsOrigin.Z,
                //player.PlayerPawn.Value.AbsRotation.X, player.PlayerPawn.Value.AbsRotation.Y, player.PlayerPawn.Value.AbsRotation.Z,
                //player.PlayerPawn.Value.AbsVelocity.X, player.PlayerPawn.Value.AbsVelocity.Y, player.PlayerPawn.Value.AbsVelocity.Z,
                //player.AngVelocity.X, player.AngVelocity.Y, player.AngVelocity.Z,
                //player.PlayerPawn.Value.AngVelocity.X, player.PlayerPawn.Value.AngVelocity.Y, player.PlayerPawn.Value.AngVelocity.Z,
                //player.PlayerPawn.Value.V_angle.X, player.PlayerPawn.Value.V_angle.Y, player.PlayerPawn.Value.V_angle.Z,
                //player.PlayerPawn.Value.EyeAngles.X, player.PlayerPawn.Value.EyeAngles.Y, player.PlayerPawn.Value.EyeAngles.Z);
            }
        }));
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

    private static Vector GetEndXYZ1(CCSPlayerController player)
    {
        // Kamera (göz) konumu
        double karakterX = player.PlayerPawn.Value.AbsOrigin.X;
        double karakterY = player.PlayerPawn.Value.AbsOrigin.Y;
        double karakterZ = player.PlayerPawn.Value.AbsOrigin.Z;

        double uzunluk = 1000;

        // Göz açıları (eye angles) radyan cinsinden olabilir, dikkat et
        double theta = player.PlayerPawn.Value.EyeAngles.X * Math.PI / 180.0;
        double phi = player.PlayerPawn.Value.EyeAngles.Y * Math.PI / 180.0;

        // A noktasını oluştur
        double pointX = karakterX + uzunluk * Math.Sin(theta) * Math.Cos(phi);
        double pointY = karakterY + uzunluk * Math.Sin(theta) * Math.Sin(phi);
        double pointZ = karakterZ + uzunluk * Math.Cos(theta);

        Console.WriteLine($"Benim koordinatlarim: {karakterX},{karakterY},{karakterZ}");
        Console.WriteLine($"Goz Acim: {player.PlayerPawn.Value.EyeAngles.X},{player.PlayerPawn.Value.EyeAngles.Y},{player.PlayerPawn.Value.EyeAngles.Z}");
        Console.WriteLine($"Kameranın baktığı noktanın koordinatları: ({pointX}, {pointY}, {pointZ})");

        return new Vector((float)pointX, (float)pointY, (float)pointZ);
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

    private void AllowLaserForWarden(CCSPlayerController player)
    {
        //foreach (var c in player.Pawn.Value!.MovementServices!.Buttons.ButtonStates)
        //{
        //    if (c == FButtonIndex)
        //    {
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

        Vector playerPosition = player.PlayerPawn?.Value.CBodyComponent?.SceneNode?.AbsOrigin;
        QAngle viewAngles = player.PlayerPawn.Value.EyeAngles;

        if (IsPlayerCloseToTarget(player, end, player.PlayerPawn.Value!.AbsOrigin, 40))
        {
            //DetachGrapple(player);
            return;
        }
        var angleDifference = CalculateAngleDifference(new Vector(viewAngles.X, viewAngles.Y, viewAngles.Z), end - playerPosition);
        if (angleDifference > 180.0f)
        {
            //DetachGrapple(player);
            Console.WriteLine($"Player {player.PlayerName} looked away from the grapple target.");
            return;
        }
        var laser = DrawLaser(start, end);
        PullPlayer(player, end, playerPosition, viewAngles);

        if (IsPlayerCloseToTarget(player, end, playerPosition, 40))
        {
            //DetachGrapple(player);
        }
        //if (laser != null)
        //{
        //    var velocity = player.PlayerPawn.Value.AbsVelocity;
        //    var res = HookThere(start, end, velocity);

        //    //player.Teleport(res.Position, player.PlayerPawn.Value.AbsRotation, res.Velocity);
        //    player.PlayerPawn.Value.Teleport(player.PlayerPawn.Value.AbsOrigin, player.PlayerPawn.Value.AbsRotation, res);
        //}
        //LasersEntry(x, y, z);
        return;
        //}
        //}
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

    private Vector CalculateForwardVector(Vector viewAngles)
    {
        if (viewAngles == null)
        {
            return new Vector(0, 0, 0);
        }

        float pitch = viewAngles.X * (float)Math.PI / 180.0f;
        float yaw = viewAngles.Y * (float)Math.PI / 180.0f;

        float x = (float)(Math.Cos(pitch) * Math.Cos(yaw));
        float y = (float)(Math.Cos(pitch) * Math.Sin(yaw));
        float z = (float)(-Math.Sin(pitch));

        return new Vector(x, y, z);
    }

    private Vector CalculateRightVector(Vector viewAngles)
    {
        if (viewAngles == null)
        {
            return new Vector(0, 0, 0);
        }

        float yaw = (viewAngles.Y - 90.0f) * (float)Math.PI / 180.0f;

        float x = (float)Math.Cos(yaw);
        float y = (float)Math.Sin(yaw);
        float z = 0.0f;

        return new Vector(x, y, z);
    }

    private void PullPlayer(CCSPlayerController player, Vector grappleTarget, Vector playerPosition, QAngle viewAngles)
    {
        if (player == null || player.PlayerPawn == null || player.PlayerPawn.Value.CBodyComponent == null || playerPosition == null || !player.IsValid || !player.PawnIsAlive)
        {
            Console.WriteLine("Player is null.");
            return;
        }

        if (player.PlayerPawn.Value.CBodyComponent.SceneNode == null)
        {
            Console.WriteLine("SceneNode is null. Skipping pull.");
            return;
        }

        if (grappleTarget == null)
        {
            Console.WriteLine("Grapple target is null.");
            return;
        }

        var direction = grappleTarget - playerPosition;
        var distance = direction.Length();
        direction = new Vector(direction.X / distance, direction.Y / distance, direction.Z / distance); // Normalize manually
        float grappleSpeed = Config.GrappleSpeed;

        var buttons = player.Buttons;
        if (buttons == null) return;

        float adjustmentFactor = 0.5f;

        var forwardVector = CalculateForwardVector(new Vector(viewAngles.X, viewAngles.Y, viewAngles.Z));
        var rightVector = CalculateRightVector(new Vector(viewAngles.X, viewAngles.Y, viewAngles.Z));

        if ((buttons & PlayerButtons.Moveright) != 0)
        {
            direction += rightVector * adjustmentFactor;
        }
        else if ((buttons & PlayerButtons.Moveleft) != 0)
        {
            direction -= rightVector * adjustmentFactor;
        }

        direction = new Vector(direction.X / direction.Length(), direction.Y / direction.Length(), direction.Z / direction.Length());

        var newVelocity = new Vector(
            direction.X * grappleSpeed,
            direction.Y * grappleSpeed,
            direction.Z * grappleSpeed
        );

        if (player.PlayerPawn.Value.AbsVelocity != null)
        {
            player.PlayerPawn.Value.AbsVelocity.X = newVelocity.X;
            player.PlayerPawn.Value.AbsVelocity.Y = newVelocity.Y;
            player.PlayerPawn.Value.AbsVelocity.Z = newVelocity.Z;
        }
        else
        {
            Console.WriteLine("AbsVelocity is null.");
            return;
        }

        //if (playerGrapples[player.Slot].GrappleWire != null)
        //{
        //    playerGrapples[player.Slot].GrappleWire.Teleport(playerPosition, new QAngle(0, 0, 0), new Vector(0, 0, 0));
        //}
        //else
        //{
        //    Console.WriteLine("GrappleWire is null.");
        //}
    }

    private float CalculateAngleDifference(Vector angles1, Vector angles2)
    {
        if (angles1 == null || angles2 == null)
        {
            return 0.0f;
        }

        float pitchDiff = Math.Abs(angles1.X - angles2.X);
        float yawDiff = Math.Abs(angles1.Y - angles2.Y);

        pitchDiff = pitchDiff > 180.0f ? 360.0f - pitchDiff : pitchDiff;
        yawDiff = yawDiff > 180.0f ? 360.0f - yawDiff : yawDiff;

        return Math.Max(pitchDiff, yawDiff);
    }

    private bool IsPlayerCloseToTarget(CCSPlayerController player, Vector grappleTarget, Vector playerPosition, float thresholdDistance)
    {
        if (player == null || grappleTarget == null || playerPosition == null)
        {
            return false;
        }

        var direction = grappleTarget - playerPosition;
        var distance = direction.Length();

        return distance < thresholdDistance;
    }

    private static Vector HookThere(Vector start, Vector end, Vector velocity)
    {
        // Başlangıç noktası
        float x0 = start.X, y0 = start.Y, z0 = start.Z;

        // Bitiş noktası
        float xf = end.X, yf = end.Y, zf = end.Z;

        // Başlangıç hızı
        //float vx0 = velocity.X, vy0 = velocity.Y, vz0 = velocity.Z;
        // Başlangıç hızını artırmak için bir faktör
        float speedFactor = 1.3f;

        // Başlangıç hızını güncelle
        float vx0 = velocity.X * speedFactor;
        float vy0 = velocity.Y * speedFactor;
        float vz0 = velocity.Z * speedFactor;

        return new Vector(vx0, vy0, vz0);
        //return new Vector(vx0, vy0, vz0);
    }

    private static (Vector Position, Vector Velocity) HookThere2(Vector start, Vector end, Vector velocity)
    {
        // Başlangıç noktası
        float x0 = start.X, y0 = start.Y, z0 = start.Z;

        // Bitiş noktası
        float xf = end.X, yf = end.Y, zf = end.Z;

        // Başlangıç hızı
        float vx0 = velocity.X, vy0 = velocity.Y, vz0 = velocity.Z;

        // Maksimum ivme
        //float a_max = 1000f;

        // Ivme başlangıcı
        float ax0 = 1f, ay0 = 1f, az0 = 1f;

        // Zaman adımı
        float deltaT = 1f;

        // Zaman
        float t = 0;

        // Hesaplamalara başla
        //while (t < 10) // 10 saniye boyunca hesapla (istenen süreyi değiştirebilirsiniz)
        {
            // Ivme sınırlaması
            //float ax = Math.Min(ax0, a_max);
            //float ay = Math.Min(ay0, a_max);
            //float az = Math.Min(az0, a_max);
            float ax = ax0;
            float ay = ay0;
            float az = az0;

            // Hız güncellemesi
            vx0 += ax * deltaT;
            vy0 += ay * deltaT;
            vz0 += az * deltaT;

            // Konum güncellemesi
            x0 += vx0 * deltaT;
            y0 += vy0 * deltaT;
            z0 += vz0 * deltaT;

            // Zamanı artır
            t += deltaT;

            // Sonuçları yazdır
            Console.WriteLine($"Time: {t:F2} sec, Position: ({x0:F2}, {y0:F2}, {z0:F2}), Velocity: ({vx0:F2}, {vy0:F2}, {vz0:F2})");
        }
        return (new Vector(x0, y0, z0), new Vector(vx0, vy0, vz0));
    }
}