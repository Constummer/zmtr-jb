using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePassPremium_Level10 : BattlePassPremiumBase
    {
        [JsonIgnore]
        public const int CTKill = 70;

        [JsonIgnore]
        public const int Jump = 25_000;

        public int CurrentCtKill { get; set; } = 0;
        public int CurrentJump { get; set; } = 0;

        public BattlePassPremium_Level10() : base(10, 30, 2000, 500)
        {
        }

        internal override void OnEventPlayerJump()
        {
            CurrentJump++;
            base.OnEventPlayerJump();
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
                CurrentCtKill >= CTKill &&
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
            menu.AddMenuOption($"{CurrentJump}/{Jump} Zıplama", null, true);
            menu.AddMenuOption($"{CurrentCtKill}/{CTKill} {CT_CamelCase} Kill", null, true);
        }
    }
}