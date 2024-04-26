using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level15 : BattlePassBase
    {
        [JsonIgnore]
        public const int Mag7Kill = 50;

        [JsonIgnore]
        public const int TWin = 150;

        public int CurrentMag7Kill { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePass_Level15() : base(15, 170, 2250, 0)
        {
        }
    }
}