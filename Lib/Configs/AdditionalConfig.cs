using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class AdditionalConfig
    {
        [JsonPropertyName("KomPermName")]
        public string KomPermName { get; set; } = "@css/komutcu";

        [JsonPropertyName("KomWeeklyCredit")]
        public int KomWeeklyCredit { get; set; } = 500;

        [JsonPropertyName("ParachuteEnabled")]
        public bool ParachuteEnabled { get; set; } = true;

        [JsonPropertyName("ParachuteModelEnabled")]
        public bool ParachuteModelEnabled { get; set; } = true;

        [JsonPropertyName("HideFootsOnConnect")]
        public bool HideFootsOnConnect { get; set; } = true;

        [JsonPropertyName("CustomRespawnEnabled")]
        public bool CustomRespawnEnabled { get; set; } = true;

        [JsonPropertyName("GrappleSpeed")]
        public float GrappleSpeed { get; set; } = 1500.0f;

        [JsonPropertyName("DiscordWChangeNotifyUrl")]
        public string DiscordWChangeNotifyUrl { get; set; } = "https://discord.com/api/webhooks/1199428821863104514/oNfcxm6pDO2wyeTPm5L75X4q9OHgw4QRNniAQXcAZTqiWlBsSRqljG1UjXunP-NMoeMK";

        [JsonPropertyName("RoundEndStartCommands")]
        public List<string> RoundEndStartCommands { get; set; } = new(){
            "mp_respawn_on_death_t 0",
            "mp_respawn_on_death_ct 0",
            "sv_enablebunnyhopping 1",
            "sv_autobunnyhopping 1",
            "sv_maxspeed 320",
            "mp_teammates_are_enemies 0",
            "sv_gravity 800"
        };
    }
}