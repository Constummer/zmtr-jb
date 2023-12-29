using CounterStrikeSharp.API.Core;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    #region Sounds

    public class Sounds
    {
        [JsonPropertyName("WardenEnterSound")]
        public string WardenEnterSound { get; set; } = "sounds/zmtr_warden/wzenter.vsnd_c";

        [JsonPropertyName("WardenLeaveSound")]
        public string WardenLeaveSound { get; set; } = "sounds/zmtr_warden/wzleave.vsnd_c";

        [JsonPropertyName("LastAliveTSound")]
        public string LastAliveTSound { get; set; } = "sounds/zmtr_lr/lr1.vsnd_c";

        [JsonPropertyName("FreezeOrUnfreezeSound")]
        public string FreezeOrUnfreezeSound { get; set; } = "sounds/zmtr_freeze/freeze.vsnd_c";

        [JsonPropertyName("LrStartSound")]
        public string LrStartSound { get; set; } = "sounds/zmtr/bell.vsnd_c";
    }

    #endregion Sounds

    public class JailbreakExtrasConfig : BasePluginConfig
    {
        [JsonPropertyName("HideFootsOnConnect")]
        public bool HideFootsOnConnect { get; set; } = true;

        [JsonPropertyName("GrappleSpeed")]
        public float GrappleSpeed { get; set; } = 1500.0f;

        [JsonPropertyName("Sounds")]
        public Sounds Sounds { get; set; } = new Sounds();

        #region Parachute

        [JsonPropertyName("ParachuteEnabled")] public bool ParachuteEnabled { get; set; } = true;
        [JsonPropertyName("ParachuteDecreaseVec")] public float ParachuteDecreaseVec { get; set; } = 50;
        [JsonPropertyName("ParachuteLinear")] public bool ParachuteLinear { get; set; } = true;
        [JsonPropertyName("ParachuteFallSpeed")] public float ParachuteFallSpeed { get; set; } = 100;

        #endregion Parachute

        #region Credit Releated

        [JsonPropertyName("SaveCreditTimerEveryXSecond")]
        public short SaveCreditTimerEveryXSecond { get; set; } = 60;

        [JsonPropertyName("RetrieveCreditEveryCTKill")]
        public short RetrieveCreditEveryCTKill { get; set; } = 5;

        [JsonPropertyName("RetrieveCreditEveryTKill")]
        public short RetrieveCreditEveryTKill { get; set; } = 1;

        [JsonPropertyName("RetrieveCreditEveryXMin")]
        public short RetrieveCreditEveryXMin { get; set; } = 5 * 60;

        [JsonPropertyName("RetrieveCreditEveryXMinReward")]
        public short RetrieveCreditEveryXMinReward { get; set; } = 5;

        #endregion Credit Releated

        #region BurryColor

        [JsonIgnore]
        public Color BuryColor { get; set; } = Color.FromArgb(255, 0, 0, 255);//when players burry, freeze they will color this

        [JsonPropertyName("BurryColorR"), Range(0, 255)]
        public short BurryColorR { get; set; } = 0;

        [JsonPropertyName("BurryColorG"), Range(0, 255)]
        public short BurryColorG { get; set; } = 0;

        [JsonPropertyName("BurryColorB"), Range(0, 255)]
        public short BurryColorB { get; set; } = 255;

        #endregion BurryColor

        #region Laser

        [JsonPropertyName("LaserRadius")]
        public int LaserRadius { get; set; } = 75;

        [JsonPropertyName("LaserWidth")]
        public int LaserWidth { get; set; } = 2;

        [JsonPropertyName("LaserEdgeCount")]
        public int LaserEdgeCount { get; set; } = 100;

        [JsonIgnore]
        public Color LaserColor { get; set; } = Color.FromArgb(255, 153, 255, 255);//when commander places marker etc, it will be this color

        [JsonPropertyName("LaserColorR"), Range(0, 255)]
        public short LaserColorR { get; set; } = 153;

        [JsonPropertyName("LaserColorG"), Range(0, 255)]
        public short LaserColorG { get; set; } = 255;

        [JsonPropertyName("LaserColorB"), Range(0, 255)]
        public short LaserColorB { get; set; } = 255;

        #endregion Laser

        #region Models

        [JsonPropertyName("RandomTModels")]
        public List<string> RandomTModels { get; set; } = new(){
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_varianta.vmdl",
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantb.vmdl",
            "characters\\models\\tm_jumpsuit\\tm_jumpsuit_variantc.vmdl",
        };

        [JsonPropertyName("RandomCTModels")]
        public List<string> RandomCTModels { get; set; } = new()
        {
            "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantc.vmdl",
            "characters\\models\\ctm_swat\\ctm_swat_variante.vmdl",
            "characters\\models\\ctm_gendarmerie\\ctm_gendarmerie_variantd.vmdl",
        };

        [JsonPropertyName("PreCacheModels")]
        public List<string> PreCacheModels { get; set; } = new()
        {
            "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_256_capped.vmdl",
            "models/coop/challenge_coin.vmdl",
            "characters/models/kaesar/mapper/mapper.vmdl",
            "characters/models/nozb1/cardboard_man_player_model/cardboard_man_player_model.vmdl",
            "characters/models/nozb1/freddy_icebear_player_model/freddy_icebear_player_model.vmdl",
            "characters/models/nozb1/huggy_male_player_model/huggy_male_player_model.vmdl",
            "characters/models/nozb1/maalik_infernal_player_model/maalik_infernal_player_model.vmdl",
            "characters/models/nozb1/pepe_player_model/pepe_player_model.vmdl",
            "characters/models/kolka/yoru/yoru.vmdl",
            "characters/models/stepanof/stalker_shrek/shrek.vmdl",
            "characters/models/nozb1/santa_player_model/santa_player_model.vmdl"
        };

        #endregion Models

        #region BlockedRadioCommands

        [JsonPropertyName("BlockedRadioCommands")]
        public List<string> BlockedRadioCommands { get; set; } = new List<string>()
        {
            "coverme",
            "takepoint",
            "holdpos",
            "regroup",
            "followme",
            "takingfire",
            "go",
            "fallback",
            "sticktog",
            "getinpos",
            "stormfront",
            "report",
            "roger",
            "enemyspot",
            "needbackup",
            "sectorclear",
            "inposition",
            "reportingin",
            "getout",
            "negative",
            "enemydown",
            "compliment",
            "thanks",
            "cheer",
            "go_a",
            "go_b",
            "sorry",
            "needrop",
            "playerradio",
            "playerchatwheel",
            "player_ping",
            "chatwheel_ping"
        };

        [JsonPropertyName("WardenAllowedRadioCommands")]
        public List<string> WardenAllowedRadioCommands { get; set; } = new List<string>()
        {
            "player_ping"
        };

        #endregion BlockedRadioCommands

        [JsonPropertyName("RoundEndStartCommands")]
        public List<string> RoundEndStartCommands { get; set; } = new(){
            "mp_respawn_on_death_t 0",
            "mp_respawn_on_death_ct 0",
            "sv_enablebunnyhopping 1",
            "sv_autobunnyhopping 1",
            "sv_maxspeed 320",
            "mp_teammates_are_enemies 0",
            "player_ping_token_cooldown 1",
        };
    }
}