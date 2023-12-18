using CounterStrikeSharp.API.Core;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class JailbreakExtrasConfig : BasePluginConfig
    {
        #region Credit Releated

        [JsonPropertyName("SaveCreditTimerEveryXSecond")]
        public short SaveCreditTimerEveryXSecond { get; set; } = 60;//saves credits every x seconds

        [JsonPropertyName("RetrieveCreditEveryCTKill")]
        public short RetrieveCreditEveryCTKill { get; set; } = 5;//Retrieve Credit Every CT Kill

        [JsonPropertyName("RetrieveCreditEveryTKill")]
        public short RetrieveCreditEveryTKill { get; set; } = 1;//Retrieve Credit Every T Kill

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
        public short LaserColorB { get; set; } = 0;

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
    }
}