using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level22 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 40;

        public int CurrentSut { get; set; } = 0;

        public BattlePass_Level22() : base(22, 320, 3000, 0)
        {
        }
    }
}