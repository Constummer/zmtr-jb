using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level01 : BattlePassBase
    {
        [JsonIgnore]
        public const int CtKill = 5;

        [JsonIgnore]
        public const int WKill = 1;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level01() : base(1, 10, 250, 0)
        {
        }
    }
}