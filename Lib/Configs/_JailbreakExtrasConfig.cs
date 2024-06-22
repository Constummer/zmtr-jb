using JailbreakExtras.Lib.Configs;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailbreakExtrasConfig
    {
        public JailbreakExtrasConfig()
        {
        }

        [JsonIgnore]
        public DatabaseConfig Database { get; set; } = new DatabaseConfig();

        [JsonIgnore]
        public AdditionalConfig Additional { get; set; } = new AdditionalConfig();

        [JsonIgnore]
        public MapConfig Map { get; set; } = new MapConfig();

        [JsonIgnore]
        public UnrestrictedFovConfig UnrestrictedFov { get; set; } = new UnrestrictedFovConfig();

        [JsonIgnore]
        public SoundsConfig Sounds { get; set; } = new SoundsConfig();

        [JsonIgnore]
        public LaserConfig Laser { get; set; } = new LaserConfig();

        [JsonIgnore]
        public CreditConfig Credit { get; set; } = new CreditConfig();

        [JsonIgnore]
        public BurryConfig Burry { get; set; } = new BurryConfig();

        [JsonIgnore]
        public MarketConfig Market { get; set; } = new MarketConfig();

        [JsonIgnore]
        public ParticleConfig Particle { get; set; } = new ParticleConfig();

        [JsonIgnore]
        public ParachuteConfig Parachute { get; set; } = new ParachuteConfig();

        [JsonIgnore]
        public SteamGroupConfig SteamGroup { get; set; } = new SteamGroupConfig();

        [JsonIgnore]
        public ModelConfig Model { get; set; } = new ModelConfig();

        [JsonIgnore]
        public BlockedRadioConfig BlockedRadio { get; set; } = new BlockedRadioConfig();

        [JsonIgnore]
        public DontBlockOnGaggedConfig DontBlockOnGagged { get; set; } = new DontBlockOnGaggedConfig();

        [JsonIgnore]
        public CTKitConfig CTKit { get; set; } = new CTKitConfig();

        [JsonIgnore]
        public LevelConfig Level { get; set; } = new LevelConfig();

        [JsonIgnore]
        public PubgConfig Pubg { get; set; } = new PubgConfig();
    }
}