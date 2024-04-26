using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level07 : BattlePassBase
    {
        [JsonIgnore]
        public const int NoScopeKill = 10;

        [JsonIgnore]
        public const int CTKill = 45;

        [JsonIgnore]
        public const int WKill = 10;

        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level07() : base(7, 70, 1500, 0)
        {
        }
    }
}