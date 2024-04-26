using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level21 : BattlePassBase
    {
        [JsonIgnore]
        public const int Kill = 2000;

        public int CurrentKill { get; set; } = 0;

        public BattlePass_Level21() : base(21, 310, 0, 1000)
        {
        }
    }
}