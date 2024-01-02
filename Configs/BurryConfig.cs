using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class BurryConfig
    {
        [JsonIgnore]
        public Color BuryColor { get; set; } = Color.FromArgb(255, 0, 0, 255);//when players burry, freeze they will color this

        [JsonPropertyName("BurryColorR"), Range(0, 255)]
        public short BurryColorR { get; set; } = 0;

        [JsonPropertyName("BurryColorG"), Range(0, 255)]
        public short BurryColorG { get; set; } = 0;

        [JsonPropertyName("BurryColorB"), Range(0, 255)]
        public short BurryColorB { get; set; } = 255;
    }
}