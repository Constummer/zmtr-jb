using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level17 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 30;

        [JsonIgnore]
        public const int CTWin = 45;

        public int CurrentSut { get; set; } = 0;
        public int CurrentCTWin { get; set; } = 0;

        public BattlePass_Level17() : base(17, 190, 2500, 0)
        {
        }
    }
}