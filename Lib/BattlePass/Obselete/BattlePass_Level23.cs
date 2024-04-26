using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level23 : BattlePassBase
    {
        [JsonIgnore]
        public const int Jump = 100_000;

        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level23() : base(23, 330, 3000, 0)
        {
        }
    }
}