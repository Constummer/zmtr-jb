using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras
{
    public class TgGameConfig
    {
        [JsonIgnore]
        public Dictionary<string, List<CoordinateTemplate>> PubgCoords { get; set; } = new Dictionary<string, List<CoordinateTemplate>>()
        {
            {"jb_zmtr_v1", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                }
            },
            {"jb_zmtr_v2", new List<CoordinateTemplate>()
                {
                    new("Hucre",   new VectorTemp(-535,345,-27) ),
                }
            },
        };
    }
}