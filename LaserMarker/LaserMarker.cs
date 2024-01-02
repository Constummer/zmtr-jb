﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static readonly List<CEnvBeam> Lasers = new();

    private static readonly Vector VEC_ZERO = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly QAngle ANGLE_ZERO = new QAngle(0.0f, 0.0f, 0.0f);

    private enum LaserType
    {
        Hook,
        Grab,
        Marker
    }

    private HookResult EXTRAOnPlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        if (@event == null)
            return HookResult.Continue;
        var player = @event.Userid;
        if (player.SteamID != LatestWCommandUser)
        {
            return HookResult.Continue;
        }
        LasersEntry(@event.X, @event.Y, @event.Z);

        return HookResult.Continue;
    }

    private static void ClearLasers()
    {
        if (Lasers != null)
        {
            foreach (var item in Lasers)
            {
                if (item.IsValid)
                {
                    item.Remove();
                }
            }
            Lasers.Clear();
        }
    }

    private void LasersEntry(float x, float y, float z)
    {
        ClearLasers();

        double radius = Config.Laser.MarkerRadius;
        int edgeCount = Config.Laser.MarkerEdgeCount;

        CalculateAndPrintEdges(x, y, z, radius, edgeCount);
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

            DrawLaser(start, end, LaserType.Marker, false);
        }
    }

    private CEnvBeam DrawLaser(Vector start, Vector end, LaserType laserType, bool clearAfter = true)
    {
        CEnvBeam? laser = Utilities.CreateEntityByName<CEnvBeam>("env_beam");

        if (laser == null)
        {
            return null;
        }
        switch (laserType)
        {
            case LaserType.Hook:
                laser.Render = Color.Red;
                break;

            case LaserType.Grab:
                laser.Render = Color.Pink;
                break;

            case LaserType.Marker:
                laser.Render = Color.Cyan;
                break;

            default:
                break;
        }
        laser.Width = Config.Laser.LaserWidth;

        laser.Teleport(start, ANGLE_ZERO, VEC_ZERO);
        laser.EndPos.X = end.X;
        laser.EndPos.Y = end.Y;
        laser.EndPos.Z = end.Z;

        laser.ClipStyle = BeamClipStyle_t.kMODELCLIP;
        laser.TouchType = Touch_t.touch_player_or_npc_or_physicsprop;
        laser.DispatchSpawn();
        if (clearAfter)
        {
            //oto remove - cok tatli
            AddTimer(0.1f, () =>
            {
                laser.Remove();
            });
        }
        else
        {
            if (Lasers != null)
            {
                Lasers.Add(laser);
            }
        }
        return laser;
    }
}