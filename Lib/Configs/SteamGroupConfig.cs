using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class SteamGroupConfig
    {
        [JsonPropertyName("SteamGroupId")]
        public string SteamGroupId { get; set; } = "27841608";

        [JsonPropertyName("SteamApiKey")]
        public string SteamApiKey { get; set; } = "21A6E9B4C6C467A36563460F288C4A1E";

        [JsonPropertyName("BlockedCommands")]
        public List<string> BlockedCommands { get; set; } = new List<string>()
        {
              "!knife",
               "/knife",
               "!ws",
               "/ws",
               "!skinler",
               "/skinler",
               "!yenile",
               "/yenile",
               "css_yenile",
               "css_skinler",
               "css_knife",
               "css_ws"
        };
    }
}