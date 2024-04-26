using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level19 : BattlePassBase
    {
        [JsonIgnore]
        public const int TWin = 200;

        [JsonIgnore]
        public const int M4A4Kill = 250;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentM4A4Kill { get; set; } = 0;

        public BattlePass_Level19() : base(19, 220, 3000, 1000)
        {
        }

        internal override void OnRoundTWinCommand()
        {
            CurrentTWin++;
            base.OnRoundTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time && CurrentTWin >= TWin)
            {
                base.CheckIfLevelUp(true);
            }
            else
            {
                base.CheckIfLevelUp(false);
            }
        }
    }
}