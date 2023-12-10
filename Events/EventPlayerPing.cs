using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static Vector VEC_ZERO = new Vector(0.0f, 0.0f, 0.0f);
    private static QAngle ANGLE_ZERO = new QAngle(0.0f, 0.0f, 0.0f);
    private static readonly Color CYAN = Color.FromArgb(255, 153, 255, 255);

    private void EventPlayerPing()
    {
        RegisterEventHandler<EventPlayerPing>((@event, _) =>
        {
            if (ValidateCallerPlayer(@event.Userid) == false)
            {
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
        });
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