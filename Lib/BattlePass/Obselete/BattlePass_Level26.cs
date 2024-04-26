using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level26 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 60;

        [JsonIgnore]
        public const int CTKill = 130;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level26() : base(26, 360, 3000, 500)
        {
        }
    }
}