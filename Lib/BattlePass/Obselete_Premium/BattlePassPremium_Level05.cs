using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level05 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int CtKill = 25;

        [JsonIgnore]
        public const int Jump = 10_000;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePassPremium_Level05() : base(5, 10, 1250, 500)
        {
        }

        internal override void OnEventPlayerJump(int jumpAmount)
        {
            CurrentJump += jumpAmount;
            base.OnEventPlayerJump(jumpAmount);
            CheckIfLevelUp(false);
        }

        internal override void EventCTKilled()
        {
            CurrentCtKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentJump >= Jump &&
                CurrentCtKill >= CtKill &&
                CurrentTime >= Time)
            {
                base.CheckIfLevelUp(true);
            }
            else
            {
                base.CheckIfLevelUp(false);
            }
        }

        internal override void BuildLevelMenu(CenterHtmlMenu menu)
        {
            base.BuildLevelMenu(menu);
            menu.AddMenuOption($"{CurrentCtKill}/{CtKill} {CT_CamelCase} Kill", null, true);
            menu.AddMenuOption($"{CurrentJump}/{Jump} Zıplama", null, true);
        }
    }
}