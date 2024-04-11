using CounterStrikeSharp.API.Core;
using JailbreakExtras.Lib.Configs;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailbreakExtrasConfig : BasePluginConfig
    {
        public JailbreakExtrasConfig()
        {
            Version = 10;
        }

        [JsonPropertyName("Database")]
        public DatabaseConfig Database { get; set; } = new DatabaseConfig();

        [JsonPropertyName("Additional")]
        public AdditionalConfig Additional { get; set; } = new AdditionalConfig();

        [JsonPropertyName("Map")]
        public MapConfig Map { get; set; } = new MapConfig();

        [JsonPropertyName("Map2")]
        public MapConfig2 Map2 { get; set; } = new MapConfig2();

        [JsonPropertyName("UnrestrictedFov")]
        public UnrestrictedFovConfig UnrestrictedFov { get; set; } = new UnrestrictedFovConfig();

        [JsonPropertyName("Sounds")]
        public SoundsConfig Sounds { get; set; } = new SoundsConfig();

        [JsonPropertyName("Laser")]
        public LaserConfig Laser { get; set; } = new LaserConfig();

        [JsonPropertyName("Credit")]
        public CreditConfig Credit { get; set; } = new CreditConfig();

        [JsonPropertyName("Burry")]
        public BurryConfig Burry { get; set; } = new BurryConfig();

        [JsonPropertyName("Market")]
        public MarketConfig Market { get; set; } = new MarketConfig();

        [JsonPropertyName("Particle")]
        public ParticleConfig Particle { get; set; } = new ParticleConfig();

        [JsonPropertyName("SteamGroup")]
        public SteamGroupConfig SteamGroup { get; set; } = new SteamGroupConfig();

        [JsonPropertyName("Model")]
        public ModelConfig Model { get; set; } = new ModelConfig();

        [JsonPropertyName("BlockedRadio")]
        public BlockedRadioConfig BlockedRadio { get; set; } = new BlockedRadioConfig();

        [JsonPropertyName("DontBlockOnGagged")]
        public DontBlockOnGaggedConfig DontBlockOnGagged { get; set; } = new DontBlockOnGaggedConfig();

        [JsonPropertyName("CTKit")]
        public CTKitConfig CTKit { get; set; } = new CTKitConfig();

        [JsonIgnore]
        public LevelConfig Level { get; set; } = new LevelConfig();

        [JsonIgnore]
        public TgGameConfig TgGame { get; set; } = new TgGameConfig();
    }

    public void OnConfigParsed(JailbreakExtrasConfig config)
    {
        if (config.Version < ModuleConfigVersion)
        {
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
            Logger.LogInformation($"CONFIG SURUMU ESKI, YENISINI OLUSTURUP DATABASE CONFIGI GIRMEYI UNUTMA");
        }
        _Config = Config = config;
        SetLevelPermissionDictionary();
        config.Burry.BuryColor = Color.FromArgb(config.Burry.BurryColorR, config.Burry.BurryColorG, config.Burry.BurryColorB);
        //Re-assign after adjustments
        _Config = Config = config;
    }
}