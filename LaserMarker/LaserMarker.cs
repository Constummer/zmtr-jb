using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<CEnvBeam> Lasers = new();

    public static readonly Color CYAN = Color.FromArgb(255, 153, 255, 255);

    private static LaserConfigData LaserConfig = new LaserConfigData()
    {
        Radius = 75,//marker R
        Width = 2,//marker genisligi
        EdgeCount = 100,// marker kenarindaki line sayisi
        Color = CYAN,// marker color
    };

    private HookResult EXTRAOnPlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player.SteamID != LatestWCommandUser)
        {
            player.PrintToChat($"Marker kullanabilmek icin warden olman gerekli");
            return HookResult.Continue;
        }
        foreach (var item in Lasers)
        {
            if (item.IsValid)
            {
                item.Remove();
            }
        }
        Lasers.Clear();

        double radius = LaserConfig.Radius;
        int edgeCount = LaserConfig.EdgeCount;

        CalculateAndPrintEdges(@event.X, @event.Y, @event.Z, radius, edgeCount);

        return HookResult.Continue;
    }

    [ConsoleCommand("markertemizle", "Eli yeniden baslatir")]
    public void LaserTemizle(CCSPlayerController? player, CommandInfo info)
    {
        if (player.SteamID != LatestWCommandUser)
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

    private static Vector VEC_ZERO = new Vector(0.0f, 0.0f, 0.0f);
    private static QAngle ANGLE_ZERO = new QAngle(0.0f, 0.0f, 0.0f);

    private void DrawLaser(Vector start, Vector end)
    {
        CEnvBeam? laser = Utilities.CreateEntityByName<CEnvBeam>("env_beam");

        if (laser == null)
        {
            return;
        }

        laser.Render = LaserConfig.Color;
        laser.Width = LaserConfig.Width;

        laser.Teleport(start, ANGLE_ZERO, VEC_ZERO);
        laser.EndPos.X = end.X;
        laser.EndPos.Y = end.Y;
        laser.EndPos.Z = end.Z;

        laser.DispatchSpawn();
        if (Lasers != null)
        {
            Lasers.Add(laser);
        }
    }

    public class LaserConfigData
    {
        public int EdgeCount { get; set; }
        public int Radius { get; set; }
        public int Width { get; set; }
        public Color Color { get; set; }
    }
}