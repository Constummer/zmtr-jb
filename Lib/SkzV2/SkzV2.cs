using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool IsPointInsideRectangle(Vector vector1, Vector vector2, Vector checkVector)
    {
        return vector1.X <= checkVector.X && checkVector.X <= vector2.X &&
               vector1.Y <= checkVector.Y && checkVector.Y <= vector2.Y &&
               vector1.Z <= checkVector.Z && checkVector.Z <= vector2.Z;
    }

    private static int PaintPlayersBasedOnTheirPos(CCSPlayerController x)
    {
        if (ValidateCallerPlayer(x, false) == false) return 0;
        if (_Config.Map.KzCellCoords.TryGetValue(Server.MapName, out var poses))
        {
            var vec1 = poses.Where(x => x.Text == "LeftBottom").FirstOrDefault();
            var vec2 = poses.Where(x => x.Text == "RightTop").FirstOrDefault();
            if (vec1 != null && vec2 != null)
            {
                var validVec1 = vec1.Coord.X == 0 && vec1.Coord.Y == 0 && vec1.Coord.Z == 0;
                var validVec2 = vec2.Coord.X == 0 && vec2.Coord.Y == 0 && vec2.Coord.Z == 0;

                if (validVec1 == false && validVec2 == false)
                {
                    if (IsPointInsideRectangle(vec1.Coord, vec2.Coord, x.PlayerPawn.Value.AbsOrigin))
                    {
                        if (ValidateCallerPlayer(x, false) == false) return 0;
                        SetColour(x, Color.FromArgb(0, 255, 0));
                        return 1;
                    }
                    else
                    {
                        if (ValidateCallerPlayer(x, false) == false) return 0;
                        SetColour(x, Color.FromArgb(255, 0, 0));
                        return -1;
                    }
                }
            }
        }
        if (ValidateCallerPlayer(x, false) == false) return 0;
        SetColour(x, _Config.Burry.BuryColor);
        return 0;
    }
}