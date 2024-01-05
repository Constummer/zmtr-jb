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
    }
}