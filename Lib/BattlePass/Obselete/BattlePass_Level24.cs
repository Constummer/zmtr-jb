using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level24 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 45;

        [JsonIgnore]
        public const int CTKill = 100;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level24() : base(24, 340, 0, 1000)
        {
        }
    }
}