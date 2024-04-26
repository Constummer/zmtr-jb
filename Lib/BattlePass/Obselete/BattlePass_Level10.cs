using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level10 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTKill = 70;

        [JsonIgnore]
        public const int Jump = 25_000;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level10() : base(10, 120, 2000, 500)
        {
        }
    }
}