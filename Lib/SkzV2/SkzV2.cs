using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private static bool IsPointInsideRectangle(Vector vector1, Vector vector2, Vector checkVector)
    {
        return IsComponentInsideRange(vector1.X, vector2.X, checkVector.X) &&
               IsComponentInsideRange(vector1.Y, vector2.Y, checkVector.Y) &&
               IsComponentInsideRange(vector1.Z, vector2.Z, checkVector.Z);
    }

    private static bool IsComponentInsideRange(double val1, double val2, double checkVal)
    {
        double minVal = Math.Min(val1, val2);
        double maxVal = Math.Max(val1, val2);
        return checkVal >= minVal && checkVal <= maxVal;
    }

    public static List<ulong> SkzV2FailedSteamIds { get; set; } = new();

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
                        SkzV2FailedSteamIds.Add(x.SteamID);
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