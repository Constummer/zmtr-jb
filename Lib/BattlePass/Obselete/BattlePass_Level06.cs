using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level06 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 20;

        [JsonIgnore]
        public const int CTWin = 30;

        public int CurrentSut { get; set; } = 0;

        public int CurrentCTWin { get; set; } = 0;

        public BattlePass_Level06() : base(6, 60, 1500, 0)
        {
        }
    }
}