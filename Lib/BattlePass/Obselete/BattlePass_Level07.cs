using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level07 : BattlePassBase
    {
        [JsonIgnore]
        public const int NoScopeKill = 10;

        [JsonIgnore]
        public const int CTKill = 45;

        [JsonIgnore]
        public const int WKill = 10;

        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentCtKill { get; set; } = 0;
        public int CurrentWKill { get; set; } = 0;

        public BattlePass_Level07() : base(7, 70, 1500, 0)
        {
        }

        internal override void EventCTKilled()
        {
            CurrentCtKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void EventWKilled()
        {
            CurrentWKill++;
            base.EventWKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentCtKill >= CTKill && CurrentTime >= Time && CurrentWKill >= WKill)
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