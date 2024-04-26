using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level13 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 25;

        [JsonIgnore]
        public const int TWin = 100;

        public int CurrentSut { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePass_Level13() : base(13, 150, 0, 500)
        {
        }
    }
}