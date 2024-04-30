using CounterStrikeSharp.API.Modules.Menu;
using Newtonsoft.Json;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level29 : BattlePassBase
    {
        [JsonIgnore]
        public const int AWPKill = 300;

        public int CurrentAWPKill { get; set; } = 0;

        public BattlePass_Level29() : base(29, 10, 4000, 1000)
        {
        }

        internal override void EventAWPKill()
        {
            CurrentAWPKill++;
            base.EventAWPKill();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentTime >= Time &&
                CurrentAWPKill >= AWPKill)
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
            menu.AddMenuOption($"{CurrentAWPKill}/{AWPKill} AWP Kill", null, true);
        }
    }
}