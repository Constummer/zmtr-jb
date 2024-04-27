using System.Text.Json.Serialization;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level30 : BattlePassBase
    {
        [JsonIgnore]
        public const int Sut = 50;

        [JsonIgnore]
        public const int NoScopeKill = 50;

        [JsonIgnore]
        public const int Jump = 500_000;

        public int CurrentSut { get; set; } = 0;
        public int CurrentNoScopeKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePass_Level30() : base(30, 500, 10000, 5000)
        {
        }

        internal override void EventNoScopeKill()
        {
            CurrentNoScopeKill++;
            base.EventNoScopeKill();
            CheckIfLevelUp(false);
        }

        internal override void OnSutCommand()
        {
            CurrentSut++;
            base.OnSutCommand();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentNoScopeKill >= NoScopeKill &&
                CurrentSut >= Sut &&
                CurrentTime >= Time)
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