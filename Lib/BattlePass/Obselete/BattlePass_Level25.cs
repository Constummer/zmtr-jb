using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level25 : BattlePassBase
    {
        [JsonIgnore]
        public const int CTWin = 60;

        [JsonIgnore]
        public const int CTKill = 120;

        public int CurrentCTWin { get; set; } = 0;
        public int CurrentCTKill { get; set; } = 0;

        public BattlePass_Level25() : base(25, 350, 0, 0, "oyuncu modeli")
        {
        }

        internal override void OnRoundCTWinCommand()
        {
            CurrentCTWin++;
            base.OnRoundCTWinCommand();
            CheckIfLevelUp(false);
        }

        internal override void EventCTKilled()
        {
            CurrentCTKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time && CurrentCTWin >= CTWin && CurrentCTKill >= CTKill)
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