using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class AdditionalConfig
    {
        [JsonPropertyName("KomPermName")]
        public string KomPermName { get; set; } = Perm_Komutcu;

        [JsonPropertyName("KomWeeklyCredit")]
        public int KomWeeklyCredit { get; set; } = 1000;

        [JsonPropertyName("BayramCredit")]
        public int BayramCredit { get; set; } = 500;

        [JsonPropertyName("BayramCreditStart")]
        public DateTime BayramCreditStart { get; set; } = new DateTime(2024, 10, 10);

        [JsonPropertyName("BayramCreditEnd")]
        public DateTime BayramCreditEnd { get; set; } = new DateTime(2024, 10, 12);

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

        [JsonPropertyName("UnlimitedReserver")]
        public bool UnlimitedReserver { get; set; } = false;

        [JsonPropertyName("NoBlockActive")]
        public bool NoBlockActive { get; set; } = true;

        [JsonPropertyName("BattlePassActive")]
        public bool BattlePassActive { get; set; } = true;

        [JsonPropertyName("BattlePassPremiumActive")]
        public bool BattlePassPremiumActive { get; set; } = true;

        [JsonPropertyName("TimeRewardActive")]
        public bool TimeRewardActive { get; set; } = true;

        [JsonPropertyName("WelcomeActive")]
        public bool WelcomeActive { get; set; } = true;

        [JsonPropertyName("HideMsg")]
        public bool HideMsg { get; set; } = true;

        [JsonPropertyName("CitMaxCount")]
        public int CitMaxCount { get; set; } = 128;

        [JsonPropertyName("PluginExceptionWebHook")]
        public string PluginExceptionWebHook { get; set; } = "https://discord.com/api/webhooks/1241871721234038874/fh81QDnbz0o8Pes8DHASNoQpgvGC9kbr0Wuh9hhhyeGNGl0MW4nlmnH-4CyIUlehnyr3";

        [JsonPropertyName("WelcomeImgUrl")]
        public string WelcomeImgUrl { get; set; } = "https://zmtr.org/assets/welcome.gif";

        [JsonPropertyName("WardenDcWebHook")]
        public string WardenDcWebHook { get; set; } = "https://discord.com/api/webhooks/1241871721234038874/fh81QDnbz0o8Pes8DHASNoQpgvGC9kbr0Wuh9hhhyeGNGl0MW4nlmnH-4CyIUlehnyr3";

        [JsonPropertyName("Total_T_CTDcWebHook")]
        public string Total_T_CTDcWebHook { get; set; } = "https://discord.com/api/webhooks/1241871721234038874/fh81QDnbz0o8Pes8DHASNoQpgvGC9kbr0Wuh9hhhyeGNGl0MW4nlmnH-4CyIUlehnyr3";

        [JsonPropertyName("Total_IsTop_DcWebHook")]
        public string Total_IsTop_DcWebHook { get; set; } = "https://discord.com/api/webhooks/1241871721234038874/fh81QDnbz0o8Pes8DHASNoQpgvGC9kbr0Wuh9hhhyeGNGl0MW4nlmnH-4CyIUlehnyr3";

        [JsonPropertyName("CustomSetParentLinuxSignature")]
        public string CustomSetParentLinuxSignature { get; set; } = "\\x48\\x85\\xF6\\x74\\x2A\\x48\\x8B\\x47\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\x48\\x8B\\x46\\x10\\xF6\\x40\\x31\\x02\\x75\\x2A\\xB8\\x2A\\x2A\\x2A\\x2A";

        [JsonPropertyName("CustomSetParentWindowsSignature")]
        public string CustomSetParentWindowsSignature { get; internal set; } = "\\x4D\\x8B\\xD9\\x48\\x85\\xD2\\x74\\x2A";

        [JsonPropertyName("CustomRespawnLinuxSignature")]
        public string CustomRespawnLinuxSignature { get; internal set; } = "\\x55\\x48\\x89\\xE5\\x41\\x54\\x49\\x89\\xFC\\x53\\xE8\\x?\\x?\\x?\\x?\\x41\\x80\\xBC\\x24\\x?\\x?\\x?\\x?\\x?\\x41\\xC6\\x84\\x24"
                /*"\\x55\\x48\\x89\\xE5\\x41\\x57\\x41\\x56\\x41\\x55\\x41\\x54\\x49\\x89\\xFC\\x53\\x48\\x89\\xF3\\x48\\x81\\xEC\\xC8\\x00\\x00\\x00"*/;

        [JsonPropertyName("CustomRespawnWindowsSignature")]
        public string CustomRespawnWindowsSignature { get; internal set; } = "\\x44\\x88\\x4C\\x24\\x2A\\x55\\x57";

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