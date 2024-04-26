using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level12 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 35;

        [JsonIgnore]
        public const int AWPKill = 35;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentAWPKill { get; set; } = 0;

        public BattlePass_Level12() : base(12, 140, 0, 500)
        {
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentAWPKill >= AWPKill && CurrentTime >= Time && CurrentCTWin >= CTWin)
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