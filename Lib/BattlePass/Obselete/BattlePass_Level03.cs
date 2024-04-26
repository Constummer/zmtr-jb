using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level03 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 10;

        [JsonIgnore]
        public const int TWin = 50;

        public int CurrentSut { get; set; } = 0;
        public int CurrentTWin { get; set; } = 0;

        public BattlePass_Level03() : base(3, 30, 750, 0)
        {
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            if (CurrentSut >= Sut)
            {
            }
            base.OnSutCommand();
        }
    }
}