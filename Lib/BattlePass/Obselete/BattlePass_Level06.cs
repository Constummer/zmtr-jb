using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level06 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 20;

        [JsonIgnore]
        public const int CTWin = 30;

        public int CurrentSut { get; set; } = 0;

        public int CurrentCTWin { get; set; } = 0;

        public BattlePass_Level06() : base(6, 60, 1500, 0)
        {
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentSut >= Sut && CurrentTime >= Time && CurrentCTWin >= CTWin)
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