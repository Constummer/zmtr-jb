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

    private static int PaintPlayersBasedOnTheirPos(CCSPlayerController x)
    {
        if (ValidateCallerPlayer(x, false) == false) return 0;
        if (_Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf) && conf != null && conf.KzCellCoords != null)
        {
            var vec1 = conf.KzCellCoords.Where(x => x.Text == "LeftBottom").FirstOrDefault();
            var vec2 = conf.KzCellCoords.Where(x => x.Text == "RightTop").FirstOrDefault();
            if (vec1 != null && vec2 != null)
            {
                var validVec1 = vec1.Coord.X == 0 && vec1.Coord.Y == 0 && vec1.Coord.Z == 0;
                var validVec2 = vec2.Coord.X == 0 && vec2.Coord.Y == 0 && vec2.Coord.Z == 0;

                if (validVec1 == false && validVec2 == false)
                {
                    var skzData = SkzTimeDatas.Where(y => y.SteamId == x.SteamID).FirstOrDefault();
                    if (skzData != null)
                    {
                        if (skzData.Done)
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

    private static void UpdatePlayersBasedOnTheirPos(Vector checkVector, ulong steamId)
    {
        if (checkVector == VEC_ZERO && SkzStartTime != null)
        {
            return;
        }
        if (_Config.Map.MapConfigDatums.TryGetValue(Server.MapName, out var conf) && conf != null && conf.KzCellCoords != null)
        {
            var vec1 = conf.KzCellCoords.Where(x => x.Text == "LeftBottom").FirstOrDefault();
            var vec2 = conf.KzCellCoords.Where(x => x.Text == "RightTop").FirstOrDefault();
            if (vec1 != null && vec2 != null)
            {
                var validVec1 = vec1.Coord.X == 0 && vec1.Coord.Y == 0 && vec1.Coord.Z == 0;
                var validVec2 = vec2.Coord.X == 0 && vec2.Coord.Y == 0 && vec2.Coord.Z == 0;

                if (validVec1 == false && validVec2 == false)
                {
                    if (IsPointInsideRectangle(vec1.Coord, vec2.Coord, checkVector))
                    {
                        var data = SkzTimeDatas.Where(x => x.SteamId == steamId).FirstOrDefault();
                        if (data != null)
                        {
                            data.Time = (DateTime.UtcNow - SkzStartTime.Value).TotalSeconds;
                            Server.PrintToChatAll($"{Prefix}{CC.Ol} {data.Name} {CC.W} adlı oyuncu, SKZ'yi {CC.G}{data.Time} {CC.W}saniyede bitirdi.");
                            data.Done = true;
                        }
                    }
                }
            }
        }
    }

    private static CPointWorldText SkzWallText = null;

    private static void SkzWallTextInit(VectorTemp wall, int angle)
    {
        if (SkzWallText != null && SkzWallText.IsValid)
        {
            SkzWallText.Remove();
        }
        SkzWallText = Utilities.CreateEntityByName<CPointWorldText>("point_worldtext");
        if (SkzWallText != null && SkzWallText.IsValid)
        {
            SkzWallText.MessageText = $"****SKZ SÜRELERİ****";
            SkzWallText.Enabled = true;
            SkzWallText.FontSize = 30;
            SkzWallText.Color = Color.DarkRed;
            SkzWallText.Fullbright = true;
            SkzWallText.WorldUnitsPerPx = 1.0f;
            SkzWallText.DepthOffset = 0.0f;
            SkzWallText.JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_LEFT;
            SkzWallText.JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_TOP;
            SkzWallText.ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_NONE;

            SkzWallText.Teleport(new Vector(wall.X, wall.Y, wall.Z),
                new QAngle(0, angle, 90),
                new Vector(0, 0, 0));
            SkzWallText.DispatchSpawn();
        }
    }

    private static string GetFormattedSKZPrintHtmlData(string firstLine)
    {
        var str = string.Join(" <br> ",
                SkzTimeDatas
                    .ToList()
                    .Where(x => x.Time != 0)
                    .OrderBy(x => x.Time)
                    .Take(4)
                    .Select(x => $"{x.Name} - {(x.Time)} sn"));
        return $"{firstLine} <br> {str}";
    }

    private static string GetFormattedSKZPrintData(string firstLine)
    {
        var str = string.Join("\n",
                SkzTimeDatas
                    .ToList()
                    .Where(x => x.Time != 0)
                    .OrderBy(x => x.Time)
                    .Take(4)
                    .Select(x => $"{x.Name} - {(x.Time)} sn"));
        return $"{firstLine}\n{str}";
    }

    private static List<SkzTimes> SkzTimeDatas = new();

    public class SkzTimes
    {
        public SkzTimes(ulong steamId, string name)
        {
            SteamId = steamId;
            Name = name;
        }

        public bool Done { get; set; }
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public double Time { get; set; }
    }
}