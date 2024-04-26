using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level04 : BattlePassBase
    {
        [JsonIgnore]
        public const int CtKill = 20;

        [JsonIgnore]
        public const int WKill = 5;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level04() : base(4, 40, 1000, 0)
        {
        }
    }
}