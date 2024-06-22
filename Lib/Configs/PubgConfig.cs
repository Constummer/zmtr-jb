using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class PubgConfig
    {
        [JsonIgnore]
        public Dictionary<string, List<VectorTemp>> PubgCoords { get; set; } = new Dictionary<string, List<VectorTemp>>()
        {
            {"jb_zmtr_v1", new List<VectorTemp>()
                {
                }
            }
        };
    }

    public class PubgConfigConverted
    {
        public string MapName { get; set; } = "";
        public List<VectorTemp> Data { get; set; } = new();
    }
}