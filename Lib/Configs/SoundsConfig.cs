using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SoundsConfig
    {
        [JsonPropertyName("WardenEnterSound")]
        public string WardenEnterSound { get; set; } = "sounds/zmtr_warden/wzenter.vsnd_c";

        [JsonPropertyName("WardenLeaveSound")]
        public string WardenLeaveSound { get; set; } = "sounds/zmtr_warden/wzleave.vsnd_c";

        [JsonPropertyName("LastAliveTSound")]
        public string LastAliveTSound { get; set; } = "sounds/zmtr_lr/lr1.vsnd_c";

        [JsonPropertyName("FreezeOrUnfreezeSound")]
        public string FreezeOrUnfreezeSound { get; set; } = "sounds/zmtr_freeze/freeze.vsnd_c";

        [JsonPropertyName("LrStartSound")]
        public string LrStartSound { get; set; } = "sounds/zmtr/bell.vsnd_c";

        [JsonPropertyName("TGStartSound ")]
        public string TGStartSound { get; set; } = "sounds/zmtr/bell.vsnd_c";

        [JsonPropertyName("BurnSounds")]
        public List<string> BurnSounds { get; set; } = new List<string>()
        {
            //"sounds/player/burn_damage1.vsnd_c",
            //"sounds/player/burn_damage2.vsnd_c",
            "sounds/player/burn_damage3.vsnd_c",
            "sounds/player/burn_damage4.vsnd_c",
            //"sounds/player/burn_damage5.vsnd_c",
        };
    }
}