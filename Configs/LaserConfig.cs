using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class LaserConfig
    {
        [JsonPropertyName("MarkerRadius")]
        public int MarkerRadius { get; set; } = 100;

        [JsonPropertyName("LaserWidth")]
        public int LaserWidth { get; set; } = 2;

        [JsonPropertyName("MarkerEdgeCount")]
        public int MarkerEdgeCount { get; set; } = 125;
    }
}