using CounterStrikeSharp.API.Modules.Menu;
using System.Text.Json.Serialization;
using static JailbreakExtras.JailbreakExtras;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    internal class BattlePass_Level21 : BattlePassBase
    {
        [JsonIgnore]
        public const int Kill = 2000;

        public int CurrentKill { get; set; } = 0;

        public BattlePass_Level21() : base(21, 10, 0, 1000)
        {
        }

        internal override void EventCTKilled()
        {
            CurrentKill++;
            base.EventCTKilled();
            CheckIfLevelUp(false);
        }

        internal override void EventTKilled()
        {
            CurrentKill++;
            base.EventTKilled();
            CheckIfLevelUp(false);
        }

        internal override void CheckIfLevelUp(bool completed)
        {
            if (CurrentKill >= Kill &&
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
            menu.AddMenuOption($"{CurrentKill}/{Kill} Kill", null, true);
        }
    }
}