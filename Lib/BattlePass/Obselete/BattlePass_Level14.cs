using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level14 : BattlePassBase
    {
        [JsonIgnore]
        public const int NoScopeKill = 25;

        [JsonIgnore]
        public const int CTKill = 70;

        [JsonIgnore]
        public const int WKill = 30;

        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level14() : base(14, 160, 1000, 0)
        {
        }
    }
}