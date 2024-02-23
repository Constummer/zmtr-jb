using System.Text.Json.Serialization;

namespace JailbreakExtras.Lib.Configs
{
    public class ParticleConfig
    {
        [JsonPropertyName("MarketModeller")]
        public List<ParticleModel> MarketModeller { get; set; } = new List<ParticleModel>()
        {
            new (0,  "Default", 0, true,  null ),
            new (1,  "Energy", 30000, true,  "particles/test/energy.vpcf" ),
        };
    }

    public class ParticleModel
    {
        public ParticleModel(int id, string text, int cost, bool enable, string pathToModel)
        {
            Id = id;
            Text = text;
            Cost = cost;
            Enable = enable;
            PathToModel = pathToModel;
        }

        [JsonPropertyName("Id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("Text")]
        public string Text { get; set; } = null;

        [JsonPropertyName("Cost")]
        public int Cost { get; set; } = 0;

        [JsonPropertyName("Enable")]
        public bool Enable { get; set; } = false;

        [JsonPropertyName("PathToModel")]
        public string PathToModel { get; set; } = null;
    }
}