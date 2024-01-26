using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public static bool HookDisabled { get; set; } = false;

    #region Hook

    [ConsoleCommand("hook", "af")]
    public void Hook(CCSPlayerController? player, CommandInfo info)
    {
        if (HookDisabled)
        {
            player.PrintToChat($"{Prefix}{CC.W} Hook kapalı.");
            return;
        }

        if (LatestWCommandUser != player.SteamID)
        {
            if (HookPlayers.TryGetValue(player.SteamID, out bool canUse) == false)
            {
                if (OnCommandValidater(player, true, "@css/seviye30", "@css/seviye30") == false)
                {
                    return;
                }
            }
        }
        if (!HookDisablePlayers.Contains(player.SteamID))
        {
            AllowLaserForWarden(player);
        }
    }

    [ConsoleCommand("hookver")]
    [CommandHelper(1, "<oyuncu ismi>")]
    public void HookVer(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);
        GetPlayers()
              .Where(x => GetTargetAction(x, target, null))
              .ToList()
              .ForEach(x =>
              {
                  Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncuya hook verdi.");

                  x.PrintToChat($"{Prefix}{CC.G} Konsolunuza `bind x hook` hazarak hook kullanmaya başlayabilirsiniz!");
                  _ = HookDisablePlayers.RemoveAll(y => y == x.SteamID);
                  HookPlayers[x.SteamID] = true;
              });
    }

    [ConsoleCommand("hookdisable")]
    [CommandHelper(1, "<oyuncu ismi,@t,@ct,@all,@me> [el boyunca hooku iptal eder]")]
    public void HookDisable(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }

        if (ValidateCallerPlayer(player) == false)
        {
            return;
        }
        if (info.ArgCount != 2) return;

        var target = info.ArgString.GetArg(0);
        var targetArgument = GetTargetArgument(target);

        GetPlayers()
             .Where(x => x.PawnIsAlive
                     && GetTargetAction(x, target, player!.PlayerName)
                     && x.PlayerName != "Constummer")
             .ToList()
             .ForEach(x =>
             {
                 if (HookDisablePlayers.Contains(x.SteamID) == false)
                 {
                     HookDisablePlayers.Add(x.SteamID);
                 }
                 if (targetArgument == TargetForArgument.None)
                 {
                     Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{x.PlayerName} {CC.W}adlı oyuncunun {CC.B} hookunu el boyunca aldı{CC.W}.");
                 }
             });
        if (targetArgument != TargetForArgument.None)
        {
            Server.PrintToChatAll($"{AdliAdmin(player.PlayerName)} {CC.G}{target} {CC.W}hedefinin {CC.B}hookunu el boyunca aldı{CC.W}.");
        }
    }

    [ConsoleCommand("hookal")]
    [CommandHelper(1, "<oyuncu ismi>")]
    public void HookAl(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/yonetim"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        if (info.ArgCount != 2) return;
        var target = info.ArgString.GetArg(0);
        GetPlayers()
              .Where(x => GetTargetAction(x, target, null))
              .ToList()
              .ForEach(x =>
              {
                  if (HookDisablePlayers.Contains(x.SteamID) == false)
                  {
                      HookDisablePlayers.Add(x.SteamID);
                  }
                  x.PrintToChat($"{Prefix}{CC.G} Hookunuz alındı!");
                  HookPlayers.Remove(x.SteamID, out _);
              });
    }

    [ConsoleCommand("hookac")]
    [ConsoleCommand("ha")]
    public void HookAc(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll($"{Prefix}{CC.W} Hook açıldı.");

        HookDisabled = false;
    }

    [ConsoleCommand("hookkapat")]
    [ConsoleCommand("hookkapa")]
    [ConsoleCommand("hk")]
    public void HookKapat(CCSPlayerController? player, CommandInfo info)
    {
        if (!AdminManager.PlayerHasPermissions(player, "@css/lider"))
        {
            player.PrintToChat($"{Prefix}{CC.W} Bu komut için yeterli yetkin bulunmuyor.");
            return;
        }
        Server.PrintToChatAll($"{Prefix}{CC.W} Hook el boyunca kapalı.");

        HookDisabled = true;
    }

    public static float GetRemainingRoundTime()
    {
        var gameRules = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault()?.GameRules;
        if (gameRules == null)
            return 0.0f;

        return (gameRules.RoundStartTime + gameRules.RoundTime) - Server.CurrentTime;
    }

    private void AllowLaserForWarden(CCSPlayerController player)
    {
        if (ValidateCallerPlayer(player, false) == false
            || player.PlayerPawn.Value!.AbsOrigin == null)
        {
            return;
        }
        var hookText = GetHookText();
        var warden = GetWarden();
        warden?.PrintToConsole($"{Prefix} {hookText} {player.PlayerName} Hook bastı");

        GetPlayers()
         .Where(x => AdminManager.PlayerHasPermissions(x, "@css/admin1"))
         .ToList()
         .ForEach(x => x.PrintToConsole($"{Prefix} {hookText} {player.PlayerName} Hook bastı"));

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
            return;
        }
        var angleDifference = CalculateAngleDifference(new Vector(viewAngles.X, viewAngles.Y, viewAngles.Z), end - playerPosition);
        if (angleDifference > 180.0f)
        {
            return;
        }
        var laser = DrawLaser(start, end, LaserType.Hook);
        PullPlayer(player, end, playerPosition, viewAngles);

        return;
    }

    private string GetHookText()
    {
        try
        {
            var ent = Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");
            int ctScore = ent.Where(team => team.Teamname == "CT")
                             .Select(team => team.Score)
                             .FirstOrDefault();

            int tScore = ent.Where(team => team.Teamname == "TERRORIST")
                            .Select(team => team.Score)
                            .FirstOrDefault();

            TimeSpan remainingTime = TimeSpan.FromMinutes(60) - ((DateTime.UtcNow - RoundStartTime));
            return $"{ctScore} - {tScore} | {(new DateTime(remainingTime.Ticks).ToString("mm:ss"))}";
        }
        catch (Exception)
        {
            return "";
        }
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
        float grappleSpeed = Config.Additional.GrappleSpeed;

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

    #endregion Hook
}