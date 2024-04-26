using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level02 : BattlePassBase
    {
        [JsonIgnore]
        public const int CtKill = 10;

        [JsonIgnore]
        public const int WKill = 2;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level02() : base(2, 20, 500, 0)
        {
        }
    }
}