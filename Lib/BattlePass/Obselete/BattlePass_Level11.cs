using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level11 : BattlePassBase
    {
        [JsonIgnore]
        public const int WKill = 25;

        [JsonIgnore]
        public const int NoScopeKill = 20;

        public int CurrentWKill { get; set; } = 0;
        public int CurrentNoScopeKill { get; set; } = 0;

        public BattlePass_Level11() : base(11, 130, 2000, 0)
        {
        }

        internal override void EventWKilled()
        {
            CurrentWKill++;
            base.EventWKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time && CurrentWKill >= WKill)
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