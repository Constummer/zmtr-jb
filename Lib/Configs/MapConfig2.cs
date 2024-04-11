using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class MapConfig2
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

        [JsonPropertyName("MapConfig")]
        public Dictionary<string, MapConfigDetailed> MapConfig { get; set; } = new()
        {
            { "map_jb_example.json" ,new MapConfigDetailed() },
        };
    }
}