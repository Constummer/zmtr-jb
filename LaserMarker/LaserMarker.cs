using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Xml.Linq;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<CEnvBeam> Lasers = new();

    private static Vector VEC_ZERO = new Vector(0.0f, 0.0f, 0.0f);
    private static QAngle ANGLE_ZERO = new QAngle(0.0f, 0.0f, 0.0f);

    private HookResult EXTRAOnPlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player.SteamID != LatestWCommandUser)
        {
            player.PrintToChat($"Marker kullanabilmek icin warden olman gerekli");
            return HookResult.Continue;
        }
        LasersEntry(@event.X, @event.Y, @event.Z);

        return HookResult.Continue;
    }

    private void LasersEntry(float x, float y, float z)
    {
        foreach (var item in Lasers)
        {
            if (item.IsValid)
            {
                item.Remove();
            }
        }
        Lasers.Clear();

        double radius = Config.LaserRadius;
        int edgeCount = Config.LaserEdgeCount;

        CalculateAndPrintEdges(x, y, z, radius, edgeCount);
    }

    [ConsoleCommand("markertemizle", "Eli yeniden baslatir")]
    public void LaserTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (player!.SteamID != LatestWCommandUser)
        {
            player.PrintToChat($"You must be an admin or warden to clean marker");
            return;
        }
        foreach (var item in Lasers)
        {
            item.Remove();
        }
        Lasers.Clear();
    }

    private void CalculateAndPrintEdges(double centerX, double centerY, double centerZ, double radius, int edgeCount)
    {
        double angleIncrement = 2 * Math.PI / edgeCount;

        for (int i = 0; i < edgeCount; i++)
        {
            double startAngle = i * angleIncrement;

            double endAngle = (i + 1) * angleIncrement;

            double startX = centerX + radius * Math.Cos(startAngle);
            double startY = centerY + radius * Math.Sin(startAngle);

            double endX = centerX + radius * Math.Cos(endAngle);
            double endY = centerY + radius * Math.Sin(endAngle);

            Vector start = new Vector((float)startX, (float)startY, (float)centerZ);
            Vector end = new Vector((float)endX, (float)endY, (float)centerZ);

            DrawLaser(start, end);
        }
    }

    private CEnvBeam DrawLaser(Vector start, Vector end)
    {
        CEnvBeam? laser = Utilities.CreateEntityByName<CEnvBeam>("env_beam");

        if (laser == null)
        {
            return null;
        }

        laser.Render = Config.LaserColor;
        laser.Width = Config.LaserWidth;

        laser.Teleport(start, ANGLE_ZERO, VEC_ZERO);
        laser.EndPos.X = end.X;
        laser.EndPos.Y = end.Y;
        laser.EndPos.Z = end.Z;

        laser.ClipStyle = BeamClipStyle_t.kMODELCLIP;
        laser.TouchType = Touch_t.touch_player_or_npc_or_physicsprop;
        laser.DispatchSpawn();
        if (Lasers != null)
        {
            var bune = laser.OnTouchedByEntity;

            Lasers.Add(laser);
        }
        //oto remove - cok tatli
        AddTimer(0.2f, () =>
        {
            laser.Remove();
        });
        return laser;
    }
}