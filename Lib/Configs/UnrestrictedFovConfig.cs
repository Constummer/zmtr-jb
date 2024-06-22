using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class UnrestrictedFovConfig
    {
        [JsonPropertyName("Enabled")]
        public bool Enabled { get; set; } = true;

        [JsonPropertyName("FOVMin")]
        public int FOVMin { get; set; } = 20;

        [JsonPropertyName("FOVMax")]
        public int FOVMax { get; set; } = 130;
    }
}