using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level17 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 30;

        [JsonIgnore]
        public const int CTWin = 45;

        public int CurrentSut { get; set; } = 0;
        public int CurrentCTWin { get; set; } = 0;

        public BattlePass_Level17() : base(17, 190, 2500, 0)
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