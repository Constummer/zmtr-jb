using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level27 : BattlePassBase
    {
        [JsonIgnore]
        public const int TWin = 200;

        [JsonIgnore]
        public const int SSGKill = 150;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentSSGKill { get; set; } = 0;

        public BattlePass_Level27() : base(27, 370, 3250, 0)
        {
        }
    }
}