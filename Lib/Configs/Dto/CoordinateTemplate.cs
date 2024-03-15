using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class CoordinateTemplate
    {
        public CoordinateTemplate(string text, VectorTemp coords)
        {
            Text = text;
            Coords = coords;
        }

        [JsonPropertyName("Text")]
        public string Text { get; set; } = "";

        [JsonPropertyName("Coord")]
        public VectorTemp Coords { get; set; } = new VectorTemp(0, 0, 0);

        [JsonIgnore]
        public Vector Coord { get => new Vector(Coords.X, Coords.Y, Coords.Z); }
    }
}