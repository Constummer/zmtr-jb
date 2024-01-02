using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailbreakExtrasConfig : BasePluginConfig
    {
        public JailbreakExtrasConfig()
        {
            Version = 4;
        }

        [JsonPropertyName("Additional")]
        public AdditionalConfig Additional { get; set; } = new AdditionalConfig();

        [JsonPropertyName("Sounds")]
        public SoundsConfig Sounds { get; set; } = new SoundsConfig();

        [JsonPropertyName("Laser")]
        public LaserConfig Laser { get; set; } = new LaserConfig();

        [JsonPropertyName("Credit")]
        public CreditConfig Credit { get; set; } = new CreditConfig();

        [JsonPropertyName("Burry")]
        public BurryConfig Burry { get; set; } = new BurryConfig();

        [JsonPropertyName("Model")]
        public ModelConfig Model { get; set; } = new ModelConfig();

        [JsonPropertyName("BlockedRadio")]
        public BlockedRadioConfig BlockedRadio { get; set; } = new BlockedRadioConfig();
    }
}