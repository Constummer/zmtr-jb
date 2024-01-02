using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class BlockedRadioConfig
    {
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
    }
}