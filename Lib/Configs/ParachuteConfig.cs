using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class ParachuteConfig
    {
        [JsonPropertyName("DefaultParachute")]
        public string DefaultParachute { get; set; } = "models/zmtr/special.vmdl";

        [JsonPropertyName("MarketModeller")]
        public List<ParticleModel> MarketModeller { get; set; } = new List<ParticleModel>()
        {
            new (0,  "Default", 0, true,  null ),
            new (1,  "BF2142", 15000, true,  "models/parachute/parachute_bf2142.vmdl" ),
            new (1,  "ŞEMSİYE", 15000, true,  "models/ptrunners/parachute/umbrella_big2.vmdl" ),
        };
    }
}