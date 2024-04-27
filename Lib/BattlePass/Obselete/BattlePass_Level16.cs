using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level16 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 85;

        [JsonIgnore]
        public const int Jump = 50_000;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level16() : base(16, 180, 2500, 0)
        {
        }

        internal override void OnEventPlayerJump()
        {
            CurrentJump++;
            base.OnEventPlayerJump();
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
            if (CurrentJump >= Jump &&
                CurrentTime >= Time &&
                CurrentCTWin >= CTWin)
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