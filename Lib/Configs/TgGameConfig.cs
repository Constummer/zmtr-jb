using System.Text.Json.Serialization;

namespace JailbreakExtras
{
    public class TgGameConfig
    {
        [JsonIgnore]
        public Dictionary<string, List<CoordinateTemplate>> PubgCoords { get; set; } = new Dictionary<string, List<CoordinateTemplate>>()
        {
            {"jb_zmtr_v1", new List<CoordinateTemplate>()
                {
                    new("",   new VectorTemp(-535,345,-27) ),
                }
            }
        };
    }
}