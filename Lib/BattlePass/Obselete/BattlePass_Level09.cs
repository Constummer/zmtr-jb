using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level09 : BattlePassBase
    {
        [JsonIgnore]
        public const int AK47Kill = 300;

        [JsonIgnore]
        public const int CTKill = 60;

        public int CurrentAK47Kill { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level09() : base(9, 90, 1750, 0)
        {
        }
    }
}