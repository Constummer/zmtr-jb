using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class AdditionalConfig
    {
        [JsonPropertyName("ParachuteEnabled")]
        public bool ParachuteEnabled { get; set; } = true;

        [JsonPropertyName("HideFootsOnConnect")]
        public bool HideFootsOnConnect { get; set; } = true;

        [JsonPropertyName("CustomRespawnEnabled")]
        public bool CustomRespawnEnabled { get; set; } = true;

        [JsonPropertyName("GrappleSpeed")]
        public float GrappleSpeed { get; set; } = 1500.0f;

        [JsonPropertyName("RoundEndStartCommands")]
        public List<string> RoundEndStartCommands { get; set; } = new(){
            "mp_respawn_on_death_t 0",
            "mp_respawn_on_death_ct 0",
            "sv_enablebunnyhopping 1",
            "sv_autobunnyhopping 1",
            "sv_maxspeed 320",
            "mp_teammates_are_enemies 0",
        };
    }
}