using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level20 : BattlePassBase
    {
        [JsonIgnore]
        public const int TWin = 250;

        [JsonIgnore]
        public const int KnifeKill = 500;

        public int CurrentTWin { get; set; } = 0;
        public int CurrentKnifeKill { get; set; } = 0;

        public BattlePass_Level20() : base(20, 300, 0, 1000)
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