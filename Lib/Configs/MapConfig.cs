using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class MapConfig
    {
        [JsonPropertyName("MapWorkshopIds")]
        public Dictionary<string, long> MapWorkshopIds { get; set; } = new Dictionary<string, long>()
        {
            {"jb_zmtr_v2remake", 3176846225 },
            {"jb_zmtr_triplex", 3170722498},
            {"jb_zmtr_ramazan", 3183895434},
            //{"jb_zmtr_minecraft_party ", 3168967727},
            //{"jb_zmtr_uzay ", 3164210735},
        };

        [JsonIgnore]
        public Dictionary<string, MapConfigDetailed> MapConfigDatums { get; set; } = new()
        {
            { "map_jb_example.json" ,new MapConfigDetailed() },
        };

        [JsonPropertyName("TelliSeferWorkshopId")]
        public string TelliSeferWorkshopId { get; set; } = "3239955866";

        [JsonPropertyName("PatronuKoruWorkshopId")]
        public string PatronuKoruWorkshopId { get; set; } = "3178317288";

        [JsonPropertyName("TelliSeferCodes")]
        public List<string> TelliSeferCodes { get; set; } = new()
        {
            "sv_enablebunnyhopping 0;sv_autobunnyhopping 0;",
            "mp_autoteambalance 1;",
            "mp_maxrounds 30;",
            "mp_halftime 1;",
            "mp_freezetime 5;",
            "mp_free_armor 1;",
            "mp_roundtime 3;",
            "sv_alltalk 1;",
            "sv_deadtalk 1;",
            "sv_voiceenable 1;",
            "mp_forcecamera 1;",
        };

        [JsonPropertyName("PatronuKoruCodes")]
        public List<string> PatronuKoruCodes { get; set; } = new()
        {
            "sv_enablebunnyhopping 0;sv_autobunnyhopping 0;",
            "mp_autoteambalance 1;",
            "mp_forcecamera 1;",
            "mp_free_armor 1;",
            "sv_alltalk 1;",
            "sv_deadtalk 1;",
            "sv_voiceenable 1;",
        };
    }
}