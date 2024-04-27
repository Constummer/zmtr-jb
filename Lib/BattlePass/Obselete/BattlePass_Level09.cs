using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level09 : BattlePassBase
    {
        [JsonIgnore]
        public const int AK47Kill = 300;

        [JsonIgnore]
        public const int CtKill = 60;

        public int CurrentAK47Kill { get; set; } = 0;
        public int CurrentCtKill { get; set; } = 0;

        public BattlePass_Level09() : base(9, 90, 1750, 0)
        {
        }

        internal override void EventCTKilled()
        {
            CurrentCtKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentCtKill >= CtKill && CurrentTime >= Time)
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