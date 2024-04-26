using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level28 : BattlePassBase
    {
        [JsonIgnore]
        public const int AWPKill = 300;

        public int CurrentAWPKill { get; set; } = 0;

        public BattlePass_Level28() : base(28, 380, 3500, 0)
        {
        }
    }
}