using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level29 : BattlePassBase
    {
        [JsonIgnore]
        public const int AWPKill = 300;

        public int CurrentAWPKill { get; set; } = 0;

        public BattlePass_Level29() : base(29, 390, 4000, 1000)
        {
        }
    }
}