using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level08 : BattlePassBase
    {
        [JsonIgnore]
        public const int P90Kill = 100;

        [JsonIgnore]
        public const int WKill = 20;

        public int CurrentP90Kill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level08() : base(8, 80, 1500, 0)
        {
        }
    }
}