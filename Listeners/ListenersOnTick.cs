using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;

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

                AllowLaserForWarden(player);

                if (Countdown_enable)
                {
                    player.PrintToCenterHtml(
                    $"<font color='gray'>►</font> <font class='fontSize-m' color='red'>{Time} saniye kaldı</font> <font color='gray'>◄</font>"
                    );
                }
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

    private void AllowLaserForWarden(CCSPlayerController player)
    {
        if (LatestWCommandUser == player.SteamID)
        {
            foreach (var c in player.Pawn.Value!.MovementServices!.Buttons.ButtonStates)
            {
                if (c == FButtonIndex)
                {
                    if (ValidateCallerPlayer(player, false) == false
                        || player.PlayerPawn.Value!.AbsOrigin == null)
                    {
                        break;
                    }
                    //Logger.LogInformation("af");

                    float x, y, z;
                    x = player.PlayerPawn.Value!.AbsOrigin!.X;
                    y = player.PlayerPawn.Value!.AbsOrigin!.Y;
                    z = player.PlayerPawn.Value!.AbsOrigin!.Z;
                    var start = new Vector((float)x, (float)y, (float)z);
                    var end = GetEndXYZ(player);

                    var laser = DrawLaser(start, end);

                    if (laser != null)
                    {
                        var velocity = player.PlayerPawn.Value.AbsVelocity;
                        var res = HookThere(start, end, velocity);

                        //player.Teleport(res.Position, player.PlayerPawn.Value.AbsRotation, res.Velocity);
                        player.PlayerPawn.Value.Teleport(player.PlayerPawn.Value.AbsOrigin, player.PlayerPawn.Value.AbsRotation, res);
                    }
                    //LasersEntry(x, y, z);
                    break;
                }
            }
        }
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