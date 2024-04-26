using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level05 : BattlePassBase
    {
        [JsonIgnore]
        public const int CtKill = 25;

        [JsonIgnore]
        public const int Jump = 10_000;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level05() : base(5, 50, 1250, 500)
        {
        }
    }
}